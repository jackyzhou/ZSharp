﻿using System.Linq;
using ZSharp.Framework.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.SqlDb
{
    public static class EfHelper
    {
        /// <summary>
        /// 通用的转换实体状态方法
        /// Repository模式中，Update总是失败及其解析:
        /// http://www.cnblogs.com/scy251147/p/3688844.html
        /// </summary>
        /// <typeparam name="TEntity">要操作的实体</typeparam>
        /// <param name="root">根实体</param>
        public static void ApplyChanges<TEntity>(this DbContext context, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            if (entities.IsNullOrEmpty())
            {
                return;
            }

            foreach (var entity in entities)
            {
                ApplyChange(context, entity);
            }
        }

        public static void Detach<TEntity>(this DbContext dbContext, TEntity entity)
            where TEntity : class
        {
            var objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<TEntity>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            if (exists)
            {
                objContext.Detach(foundEntity);
            }
        }

        private static void ApplyChange<TEntity>(DbContext context, TEntity root)
        {
            context.Set(root.GetType()).Add(root);
            var entries = context.ChangeTracker.Entries<Entity>().ToList();
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entity.ObjectState != ObjectStateType.PartialModified)
                {
                    entry.State = ConvertState(entity.ObjectState);
                }
                else
                {
                    if (entity.NeedUpdateList.Count() == 0)
                    {
                        continue;
                    }
                    var type = entity.GetType();
                    context.Set(type).Attach(entity);
                    var objContext = ((IObjectContextAdapter)context).ObjectContext;
                    var updateEntry = objContext.ObjectStateManager.GetObjectStateEntry(entity);

                    foreach (var prop in entity.NeedUpdateList.Keys)
                    {
                        var pList = prop.Split('.');
                        //指定屬性更新，如果屬性是值對象，則會更新整個值對象對應的字段
                        //TODO: 是否有方法可以更新指定值對象的指定字段值？
                        updateEntry.SetModifiedProperty(pList[0]);
                    }

                    //如果批量局部更新，那麼ChangeTracker會重複獲取已經設置過的entity，目前的做法是清空NeedUpdateList集合
                    entity.NeedUpdateList = new Dictionary<string, object>();
                }
            }
        }

        private static EntityState ConvertState(ObjectStateType state)
        {
            switch (state)
            {
                case ObjectStateType.Added:
                    return EntityState.Added;
                case ObjectStateType.Modified:
                case ObjectStateType.PartialModified:
                    return EntityState.Modified;
                case ObjectStateType.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
