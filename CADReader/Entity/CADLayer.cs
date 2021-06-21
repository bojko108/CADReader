using CAD.Geometry;
using RBush;
using System;
using System.Linq;
using System.Collections.Generic;
using CAD.Nomenclature;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява слой от CAD файл
    /// </summary>
    public class CADLayer
    {
        /// <summary>
        /// Име на слоя
        /// </summary>
        public string Name => this.Type.ToString();
        /// <summary>
        /// Тип на слоя
        /// </summary>
        public CADLayerType Type { get; private set; }
        /// <summary>
        /// Обекти в слоя - <see cref="CADPoint"/>, <see cref="CADLine"/>, 
        /// <see cref="CADContour"/>, <see cref="CADSymbol"/> and <see cref="CADText"/> 
        /// </summary>
        public List<ICADEntity> Entities { get; private set; }
        /// <summary>
        /// Брой обекти в слоя - <see cref="CADPoint"/>, <see cref="CADLine"/>, 
        /// <see cref="CADContour"/>, <see cref="CADSymbol"/> and <see cref="CADText"/> 
        /// </summary>
        public int EntitiesCount => this.Entities.Count;

        /// <summary>
        /// Пространствен индекс на обектите в слоя
        /// </summary>
        private readonly RBush<IGeometry> tree;

        /// <summary>
        /// Създава нов слой
        /// </summary>
        /// <param name="name">според номенклатурата - <see cref="CADLayerType"/></param>
        public CADLayer(string name)
        {
            this.Type = (CADLayerType)Enum.Parse(typeof(CADLayerType), name);

            this.tree = new RBush<IGeometry>();
            this.Entities = new List<ICADEntity>();
        }

        /// <summary>
        /// Добавя нов обект към слоя
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(ICADEntity entity)
        {
            this.Entities.Add(entity);

            if (entity.Geometry != null)
                this.tree.Insert(entity.Geometry);
        }

        /// <summary>
        /// Премахва обект от слоя
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(ICADEntity entity)
        {
            if (this.Entities.Contains(entity))
            {
                this.Entities.Remove(entity);

                if (entity.Geometry != null)
                    this.tree.Delete(entity.Geometry);
            }
        }

        /// <summary>
        /// Търсене на <see cref="ICADEntity"/> в зададеният обхват
        /// </summary>
        /// <param name="extent">Emin Nmin Emax Nmax</param>
        /// <returns></returns>
        public List<ICADEntity> Search(Extent extent)
        {
            List<string> numbers = this.tree
                 .Search(extent.ToEnvelope())
                 .Select(i => i.GUID)
                 .ToList();
            return this.Entities
                .FindAll(e => numbers.IndexOf(e.Geometry.GUID) > -1);
        }

        /// <summary>
        /// Търсене на <see cref="ICADEntity"/>, удовлетворяващи условието
        /// </summary>
        /// <param name="predicate">условие</param>
        /// <returns></returns>
        public List<ICADEntity> Search(Predicate<ICADEntity> predicate)
        {
            return this.Entities
                 .FindAll(predicate);
        }
    }
}
