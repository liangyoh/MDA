﻿using MDA.Eventing;
using System.Collections.Generic;

namespace MDA.Domain.Model
{
    /// <summary>
    /// 表示一个基于事件溯源的聚合根实体。
    /// </summary>
    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        /// <summary>
        /// 初始化一个 <see cref="EventSourcedRootEntity"/> 实例。
        /// </summary>
        public EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }
        /// <summary>
        /// 初始化一个 <see cref="EventSourcedRootEntity"/> 实例。
        /// </summary>
        /// <param name="eventStream">事件流</param>
        /// <param name="streamVersion">流版本</param>
        public EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion)
           : this()
        {
            foreach (var evt in eventStream)
            {
                When(evt);
            }
            _unmutatedVersion = streamVersion;
        }

        /// <summary>
        /// 领域事件列表
        /// </summary>
        private readonly IList<IDomainEvent> _mutatingEvents;

        /// <summary>
        /// 已变异的版本号
        /// </summary>
        private int _unmutatedVersion;
        protected int MutatedVersion
        {
            get { return _unmutatedVersion + 1; }
        }

        /// <summary>
        /// 为变异的版本号
        /// </summary>
        protected int UnmutatedVersion
        {
            get { return _unmutatedVersion; }
        }

        /// <summary>
        /// 获取领域事件列表。
        /// </summary>
        /// <returns></returns>
        public IList<IDomainEvent> GetMutatingEvents()
        {
            return _mutatingEvents;
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="e"></param>
        private void When(IDomainEvent e)
        {
            (this as dynamic).Apply(e);
        }

        /// <summary>
        /// 应用领域事件
        /// </summary>
        /// <param name="e"></param>
        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            When(e);
        }
    }
}
