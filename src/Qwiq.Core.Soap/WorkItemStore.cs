using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Qwiq.Credentials;
using Microsoft.Qwiq.Exceptions;
using Microsoft.TeamFoundation.WorkItemTracking.Common;

using TfsWorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Microsoft.Qwiq.Soap
{
    /// <summary>
    ///     Wrapper around the TFS WorkItemStore. This exists so that every agent doesn't need to reference
    ///     all the TFS libraries.
    /// </summary>
    internal class WorkItemStore : IWorkItemStore
    {
        private readonly Lazy<WorkItemLinkTypeCollection> _linkTypes;

        private readonly Lazy<IQueryFactory> _queryFactory;

        private readonly Lazy<IInternalTfsTeamProjectCollection> _tfs;

        private readonly Lazy<TfsWorkItem.WorkItemStore> _workItemStore;

        private readonly Lazy<IProjectCollection> _projects;

        internal WorkItemStore(
            TeamFoundation.Client.TfsTeamProjectCollection tfsNative,
            Func<WorkItemStore, IQueryFactory> queryFactory,
            int pageSize = PageSizeLimits.MaxPageSize)
            : this(
                () => ExceptionHandlingDynamicProxyFactory.Create<IInternalTfsTeamProjectCollection>(
                    new TfsTeamProjectCollection(tfsNative)),
                queryFactory, pageSize)
        {
        }

        internal WorkItemStore(
            Func<IInternalTfsTeamProjectCollection> tpcFactory,
            Func<TfsWorkItem.WorkItemStore> wisFactory,
            Func<WorkItemStore, IQueryFactory> queryFactory,
            int pageSize = PageSizeLimits.MaxPageSize)
        {
            if (tpcFactory == null) throw new ArgumentNullException(nameof(tpcFactory));
            if (wisFactory == null) throw new ArgumentNullException(nameof(wisFactory));
            if (queryFactory == null) throw new ArgumentNullException(nameof(queryFactory));

            if (pageSize < PageSizeLimits.DefaultPageSize || pageSize > PageSizeLimits.MaxPageSize)
                throw new PageSizeRangeException();

            _tfs = new Lazy<IInternalTfsTeamProjectCollection>(tpcFactory);
            _workItemStore = new Lazy<TfsWorkItem.WorkItemStore>(wisFactory);
            _queryFactory = new Lazy<IQueryFactory>(() => queryFactory(this));

            _linkTypes = new Lazy<WorkItemLinkTypeCollection>(
                () =>
                    {
                        return new WorkItemLinkTypeCollection(
                            _workItemStore.Value.WorkItemLinkTypes.Select(item => new WorkItemLinkType(item)));
                    });

            _projects = new Lazy<IProjectCollection>(()=>new ProjectCollection(_workItemStore.Value.Projects));

            PageSize = pageSize;
        }

        internal WorkItemStore(
            Func<IInternalTfsTeamProjectCollection> tpcFactory,
            Func<WorkItemStore, IQueryFactory> queryFactory,
            int pageSize = PageSizeLimits.MaxPageSize)
            : this(tpcFactory, () => tpcFactory?.Invoke()?.GetService<TfsWorkItem.WorkItemStore>(), queryFactory, pageSize)
        {
        }

        public int PageSize { get; }

        public ClientType ClientType => ClientType.Soap;

        public TfsCredentials AuthorizedCredentials => _tfs.Value.AuthorizedCredentials;

        internal TfsWorkItem.WorkItemStore NativeWorkItemStore => _workItemStore.Value;

        public IFieldDefinitionCollection FieldDefinitions => ExceptionHandlingDynamicProxyFactory
            .Create<IFieldDefinitionCollection>(
                new FieldDefinitionCollection(_workItemStore.Value.FieldDefinitions));

        public IProjectCollection Projects => _projects.Value;

        public ITfsTeamProjectCollection TeamProjectCollection => _tfs.Value;

        public TimeZone TimeZone => _workItemStore.Value.TimeZone;

        public string UserDisplayName => _workItemStore.Value.UserDisplayName;

        public string UserAccountName => TeamProjectCollection.AuthorizedIdentity.GetUserAlias();

        public string UserSid => _workItemStore.Value.UserSid;

        public WorkItemLinkTypeCollection WorkItemLinkTypes => _linkTypes.Value;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<IWorkItem> Query(string wiql, bool dayPrecision = true)
        {
            try
            {
                var query = _queryFactory.Value.Create(wiql, dayPrecision);
                return query.RunQuery();
            }
            catch (TfsWorkItem.ValidationException ex)
            {
                throw new ValidationException(ex);
            }
        }

        public IEnumerable<IWorkItem> Query(IEnumerable<int> ids, DateTime? asOf = null)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            var ids2 = (int[])ids.ToArray().Clone();
            if (!ids2.Any()) return Enumerable.Empty<IWorkItem>();

            try
            {
                var query = _queryFactory.Value.Create(ids2, asOf);
                return query.RunQuery();
            }
            catch (TfsWorkItem.ValidationException ex)
            {
                throw new ValidationException(ex);
            }
        }

        public IWorkItem Query(int id, DateTime? asOf = null)
        {
            return Query(new[] { id }, asOf).SingleOrDefault();
        }

        public IEnumerable<IWorkItemLinkInfo> QueryLinks(string wiql, bool dayPrecision = true)
        {
            try
            {
                var query = _queryFactory.Value.Create(wiql, dayPrecision);
                return query.RunLinkQuery();
            }
            catch (TfsWorkItem.ValidationException ex)
            {
                throw new ValidationException(ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) if (_tfs.IsValueCreated) _tfs.Value?.Dispose();
        }
    }
}