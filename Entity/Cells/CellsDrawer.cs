using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Cube
{
    class CellsDrawer
    {
        SLDManager application;
        private SketchManager swSketchManager;
        private SelectionMgr swSelMgr;

        private double acc = 1000.0;
        Cells cellObj;
        BodyParam body;
        BodyDrawer bodyDrawer;
        Array faces; // Список граней тела

        Feature activeSketch;
        Feature cut;

        public CellsDrawer(SLDManager app, BodyParam body, BodyDrawer bodyDrawer)
        {
            application = app;
            this.body = body;
            this.bodyDrawer = bodyDrawer;
            // Получает ISketchManager объект, который позволяет получить доступ к процедурам эскиза
            swSketchManager = application.swModel.SketchManager;
            // Получает ISelectionMgr объект для данного документа, что делает выбранный объект доступным
            swSelMgr = (SelectionMgr)application.swModel.SelectionManager;
        }

        public void drawCells()
        {
            //получить грани бобышки
            faces = bodyDrawer.body3d.GetFaces();
            //выбрать вторую (вверх бобышки)
            var ent = faces.GetValue(1) as Entity;
            //выбрать верхнюю грань
            ent.Select(true);
            //добавить на неё эскиз
            swSketchManager.InsertSketch(false);

            // Получаем объект эскиза, на котором будем рисовать
            activeSketch = application.swModel.GetActiveSketch2();

            //cells
            //Определяем положение центра верхней левой ячейки относительно центра плоскости (0, 0, 0)
            double x_current = ((-body.GetWidth()) / 2.0 + (cellObj.GetK() + cellObj.GetHoleWidth() / 2.0)) / acc;
            double y_current = ((body.GetHeight() / 2.0) - (cellObj.GetK() + (cellObj.GetHoleHeight() / 2.0))) / acc;
            //Определяем положение правого нижнего угла ячейки
            double x_end = (((-body.GetWidth()) / 2.0) + (cellObj.GetK() + cellObj.GetHoleWidth())) / acc;
            double y_end = ((body.GetHeight() / 2.0) - (cellObj.GetK() + cellObj.GetHoleHeight())) / acc;

            //Запоминаем позицию для дальнейшего использования
            double leftHoleCenterX = x_current, leftHoleCenterY = y_current;

            //Сдвиг, расстояние от центра одной ячейки до центра другой по координате Х
            double delta = (cellObj.GetHoleWidth() + cellObj.GetK()) / acc;

            //Определяем количество ячеек в зависимости от итерации
            int row = cellObj.CellsInRowNumber(), collumn = cellObj.CellsInColumnNumber();

            for (int i = 0; i < row; i++)
            {
                //Рисуем первую в ряду ячейку
                application.swModel.SketchManager.CreateCenterRectangle(x_current, y_current, 0, x_end, y_end, 0);
                //Рисуем остальные
                for (int j = 1; j < collumn; j++)
                {
                    x_current = x_current + delta;
                    x_end = x_end + delta;
                    application.swModel.SketchManager.CreateCenterRectangle(x_current, y_current, 0, x_end, y_end, 0);
                }
                //Возвращаемся к первой ячейке
                x_current = leftHoleCenterX;
                //Сдвигаемся по координате Y
                y_current = leftHoleCenterY - (cellObj.GetK() + cellObj.GetHoleHeight()) / acc;
                //Запоминаем координаты
                leftHoleCenterX = x_current; leftHoleCenterY = y_current;
                //Определяем положение правого нижнего угла ячейки
                x_end = x_current + (cellObj.GetHoleWidth() / 2.0) / acc;
                y_end = y_current + (cellObj.GetHoleHeight() / 2.0) / acc;
            }

            //Получаем объект "вырез"
            cut = featureCut(cellObj.GetHoleLenght() / acc);
            application.swModel.ClearSelection();
        }

        public void SetCells(Cells cells) { cellObj = cells; }

        /// <summary>
        /// Удаление отвкерстий
        /// </summary>
        public void deleteCells()
        {
            cut.Select(true);
            application.swModel.DeleteSelection(true);
            activeSketch.Select(true);
            application.swModel.DeleteSelection(true);
        }

        /// <summary>
        /// Вырезать по контуру
        /// </summary>
        /// <param name="deepth">глубина выреза</param>
        /// <param name="flip">вырезать внутри контура или снаружи</param>
        /// <param name="mode">режим выреза</param>
        /// <returns>объект "вырез"</returns>
        private Feature featureCut(double deepth, bool flip = false, swEndConditions_e mode = swEndConditions_e.swEndCondBlind)
        {
            return application.swModel.FeatureManager.FeatureCut2(true, flip, false, (int)mode, (int)mode,
                deepth, 0, false, false, false, false, 0, 0, false, false, false, false, false,
                false, false, false, false, false);
        }
    }
}
