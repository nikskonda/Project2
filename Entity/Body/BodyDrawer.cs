using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Cube
{
    class BodyDrawer
    {
        BodyParam body;
        SLDManager application;
        private SketchManager swSketchManager;
        private SelectionMgr swSelMgr;

        private double acc = 1000.0;

        public Feature body3d;
        Feature sketch;

        public BodyDrawer(SLDManager app, BodyParam body)
        {
            application = app;
            // Получает ISketchManager объект, который позволяет получить доступ к процедурам эскиза
            swSketchManager = application.swModel.SketchManager;
            // Получает ISelectionMgr объект для данного документа, что делает выбранный объект доступным
            swSelMgr = (SelectionMgr)application.swModel.SelectionManager;
            this.body = body;
        }

        /// <summary>
        /// Рисует тело, в котором будут отверстия
        /// </summary>
        /// <returns>Возвращает объект, на котором будут сделаны ячейки</returns>
        public void drawBody()
        {
            //получем ссылку на интерфейс, ответственный за рисование
            swSketchManager = application.swModel.SketchManager;
            application.swModel.Extension.SelectByID2("Сверху", "PLANE", 0, 0, 0, false, 0, null, 0); //выбрал плоскость  

            swSketchManager.InsertSketch(false);
            sketch = application.swModel.GetActiveSketch2();
            //создать основание
            var rect = swSketchManager.CreateCenterRectangle(0, 0, 0, body.GetWidth() / acc / 2.0, body.GetHeight() / acc / 2.0, 0);
            //очистить буфер выбранных элементов
            application.swModel.ClearSelection();

            //вытянуть бобышку
            Feature feature = featureExtrusion(body.GetLenght() / acc);
            application.swModel.ClearSelection();

            body3d = feature;
        }

        /// <summary>
        /// Вытянуть бобышку
        /// </summary>
        /// <param name="deepth">высота выдавливания</param>
        /// <param name="dir">направление выдвливания</param>
        /// <returns>объект бобышка</returns>
        private Feature featureExtrusion(double deepth, bool dir = false)
        {
            return application.swModel.FeatureManager.FeatureExtrusion2(true, false, dir,
                (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                deepth, 0, false, false, false, false, 0, 0, false, false, false, false, true,
                true, true, 0, 0, false);
        }

        /// <summary>
        /// Удаление объекта класса Feature
        /// </summary>
        /// <param name="thing">Удаляеммый объект</param>
        public void deleteBody()
        {
            body3d.Select(true);
            application.swModel.DeleteSelection(true);
            sketch.Select(true);
            application.swModel.DeleteSelection(true);
        }

        /// <summary>
        /// Взовращает список граней тела
        /// </summary>
        /// <returns></returns>
        public Array GetFacesArray() {
            return body3d.GetFaces();
        }
    }
}
