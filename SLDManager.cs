using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;

namespace Cube
{ 
    class SLDManager
    {
        public SldWorks swApp;
        public IModelDoc2 swModel;

        /// <summary>
        /// Функция проверяет, включен ли SolidWorks
        /// </summary>
        /// <returns>Возвращает true, если программа готова к работе</returns>
        public Boolean GetSolidworks()
        {
            try
            {
                // Присваиваем переменной ссылку на запущенный solidworks (по названию)
                swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch
            {
                // Отображает окно сообщения с заданным текстом
                MessageBox.Show("Не удалось найти запущенный Solidworks!");
                return false;
            }

            if (swApp.ActiveDoc == null)
            {
                // Отображает окно сообщения с заданным текстом
                MessageBox.Show("Надо открыть деталь перед использованием");
                return false;
            }
            // Присваиваем переменной ссылку на открытый активный проект в  SolidWorks
            swModel = (ModelDoc2)swApp.ActiveDoc;

            return true;
        }
    }
}
