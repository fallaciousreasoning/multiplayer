using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Collections;

namespace MultiPlayer.Core.Families
{
    public class SophisticatedFamily : IFamily
    {
        private readonly ObservableLinkedList<Entity> entities = new ObservableLinkedList<Entity>();

        private readonly Dictionary<Entity, LinkedListNode<Entity>> entityMap = new Dictionary<Entity, LinkedListNode<Entity>>();

        private readonly LinkedList<Entity> entitiesToRemove = new LinkedList<Entity>();
        private readonly LinkedList<Entity> entitiesToAdd = new LinkedList<Entity>();
        
        public SophisticatedFamily(ConstituentTypes constituentTypes)
        {
            ConstituentTypes = constituentTypes;
        }

        public void OnEntityCreated(Entity entity)
        {
            QueueAddIfMatch(entity);
        }

        public void OnEntityRemoved(Entity entity)
        {
            QueueRemove(entity);
        }

        public void OnComponentAdded(Entity entity, object component)
        {
            if (!ConstituentTypes.Contains(component.GetType())) return;

            QueueAddIfMatch(entity);
        }

        public void OnComponentRemoved(Entity entity, object component)
        {
            //If the component isn't important, ignore it
            if (!ConstituentTypes.Contains(component.GetType())) return;

            //If it was, remove it
            QueueRemove(entity);
        }

        private void QueueRemove(Entity entity)
        {
            entitiesToRemove.AddLast(entity);
        }

        private void QueueAddIfMatch(Entity entity)
        {
            entitiesToAdd.AddLast(entity);
        }

        private void AddIfMatch(Entity entity)
        {
            //If we already have this entity, don't do anything
            if (entityMap.ContainsKey(entity)) return;

            //If this entity doesn't meet our requirements, don't do anything
            if (!Matches(entity)) return;
            
            var node = entities.AddLast(entity);
            entityMap.Add(entity, node);
        }

        private void Remove(Entity entity)
        {
            if (!entityMap.ContainsKey(entity)) return;

            var node = entityMap[entity];
            entityMap.Remove(entity);

            entities.Remove(node);
        }

        private bool Matches(Entity entity)
        {
            return ConstituentTypes.TypesList.All(entity.HasComponent);
        }

        public void Maintain()
        {
            foreach (var node in entitiesToRemove) Remove(node);
            foreach (var entity in entitiesToAdd) AddIfMatch(entity);
        }

        public ConstituentTypes ConstituentTypes { get; }

        public IObservableLinkedList<Entity> Entities => entities;
    }
}
