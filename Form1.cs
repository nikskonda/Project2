using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;
using Cube.Models;

namespace Cube
{
    public partial class Form1 : Form
    {
        SldWorks swApp;
        IModelDoc2 swModel;
        private SketchManager swSketchManager;
        private SelectionMgr swSelMgr;

        SQLWorker sqlWorker;

        Cells h;

        List<Detail> details = new List<Detail>();
        Detail newDetail = new Detail();

        List<Property> detProperties = new List<Property>();
        List<Property> matProperties = new List<Property>();
       List<Property> cellProperties = new List<Property>();
        List<Property> csProperties = new List<Property>();
        List<Property> objProperties = new List<Property>();



        //Add Detail
        //List<Cell> cells;
        List<Material> materials= new List<Material>();

        public Form1(String ServerName, String DataBaseName)
        {
            sqlWorker = new SQLWorker(ServerName, DataBaseName);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cubeRadButton.Checked = true;
            multiplierCheck.Checked = true;
            IterCheck.Checked = true;


            loadDetails();

            //Add Detail
            chooseMaterial.Checked = true;

            loadMaterials();

            loadCells();

            loadDetailProperty(cmbADDetParamChoose);

            loadCSProperty(cmbCSProp);

            foreach (ParentType p in Enum.GetValues(typeof(ParentType)))
            {
                cmbChooseParentType.Items.Add(p.ToString());
            }
        }

        private void loadDetailProperty(ComboBox cmb)
        {
            detProperties.Clear();
            detProperties = sqlWorker.GetProperties(ParentType.Detail);
            cmb.Items.Clear();
            foreach (Property p in detProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadCSProperty(ComboBox cmb)
        {
            csProperties.Clear();
            csProperties = sqlWorker.GetProperties(ParentType.CelluralStructure);
            cmb.Items.Clear();
            foreach (Property p in csProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadCellProperty(ComboBox cmb)
        {
            cellProperties.Clear();
            cellProperties = sqlWorker.GetProperties(ParentType.Cell);
            cmb.Items.Clear();
            foreach (Property p in cellProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadMaterialProperty(ComboBox cmb)
        {
            matProperties.Clear();
            matProperties = sqlWorker.GetProperties(ParentType.Material);
            cmb.Items.Clear();
            foreach (Property p in matProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }



        private void loadDetails()
        {
            details.Clear();
            details = sqlWorker.GetDetails();
            foreach (Detail d in details)
            {
                cmbBoxChooseDetail.Items.Add(d.Name);
            }
        }

        private void loadMaterials()
        {
            cmbADMatChoose.Items.Clear();
            materials.Clear();
            materials = sqlWorker.GetMaterials();
            foreach (Material m in materials)
            {
                cmbADMatChoose.Items.Add(m.Name);
            }
        }
        public void loadCells()
        {
            cmbADCellChoose.Items.Clear();

            List<Cell> list = sqlWorker.GetCells();
            foreach (Cell c in list)
            {
                cmbADCellChoose.Items.Add(c.Name);
            }
        }

        public void loadCellTypes()
        {
            cmbADCellTypes.Items.Clear();
            List<CellType> list = sqlWorker.GetCellTytes();
            foreach (CellType ct in list)
            {
                cmbADCellTypes.Items.Add(ct.TypeName);
            }
        }

        private void cubeRadButton_CheckedChanged(object sender, EventArgs e)
        {
            if (cubeRadButton.Checked)
            {
                BBox.Enabled = false;
                CBox.Enabled = false;
            }
        }

        private void ParalRadButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ParalRadButton.Checked)
            {
                BBox.Enabled = true;
                CBox.Enabled = true;
            }
        }

        private void IterCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (IterCheck.Checked)
            {
                Iteration.Enabled = true;
                RowNum.Enabled = false;
                ColumnNum.Enabled = false;
            }
        }

        private void NonIter_CheckedChanged(object sender, EventArgs e)
        {
            if (NonIter.Checked)
            {
                Iteration.Enabled = false;
                RowNum.Enabled = true;
                ColumnNum.Enabled = true;
            }
        }

        private void multiplierCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (multiplierCheck.Checked)
            {
                label5.Text = "Множитель";
                label6.Text = "";
                textBox2.Enabled = false;
            }
        }

        private void fractionCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (FractionCheck.Checked)
            {
                label5.Text = "Числитель";
                label6.Text = "Знаменатель";
                textBox2.Enabled = true;
            }
        }

/*////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////*/

        private void button1_Click(object sender, EventArgs e)
        {
            double bodyWidth;
            double bodyHeight;
            double bodyLenght;

            if (cubeRadButton.Checked)
            {
                bodyWidth = Convert.ToDouble(ABox.Text.Replace('.', ','));
                bodyHeight = bodyWidth;
                bodyLenght = bodyWidth;
            }
            else
            {
                bodyWidth = Convert.ToDouble(ABox.Text.Replace('.', ','));
                bodyHeight = Convert.ToDouble(BBox.Text.Replace('.', ','));
                bodyLenght = Convert.ToDouble(CBox.Text.Replace('.', ','));
            }

            if(bodyWidth<=0 && bodyHeight<=0 && bodyLenght <= 0)
            {
                MessageBox.Show("Стороны тела не могут быть меньше нуля.");
                return;
            }

            h = new Cells(bodyWidth, bodyHeight, bodyLenght);

            if (multiplierCheck.Checked)
            {
                var buff = Convert.ToDouble(textBox1.Text.Replace('.', ','));
                if (buff >= 1) {
                    MessageBox.Show("Множитель не может быть больше 1");
                    return;
                };
                h.SetHoleVWithMultiplier(buff);
            }
            else
            {
                h.SetHoleVWithMultiplier(Convert.ToDouble(textBox1.Text.Replace('.', ',')),
                    Convert.ToDouble(textBox2.Text.Replace('.', ',')));
            }
            //MessageBox.Show(h.GetVHole().ToString());

            h.SetIterationNumber(Convert.ToInt32(Iteration.Text));
            h.KCalculation();

            foreach (Complex element in h.debugList)
            {
                textBox5.Text += element.ToString();
                textBox5.Text += System.Environment.NewLine;
            }
            textBox5.Text += System.Environment.NewLine;

        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (!GetSolidworks())
            {
                return;
            }//*/

            if (!h.isAvailable()) {
                MessageBox.Show("Невозможно начертить фигуру - не найден корень или ошибка в воде данных");
                return;
            }
            else
            {
                MessageBox.Show("Решение найдено");
                //return;
            }

            double a, b, c;
            double acc = 100.0;
            
            //получем ссылку на интерфейс, ответственный за рисование
            var sm = swModel.SketchManager;
            var fm = swModel.FeatureManager;

            swModel.Extension.SelectByID2("Сверху", "PLANE", 0, 0, 0, false, 0, null, 0); //выбрал плоскость  

            sm.InsertSketch(false);
            //создать основание
            //var rect = sm.CreateCenterRectangle(0, 0, 0, a / 2, b / 2, 0);
            var rect = sm.CreateCenterRectangle(0, 0, 0, h.GetBodyWidth()/ acc / 2.0, h.GetBodyHeight() / acc / 2.0, 0);
            //очистить буфер выбранных элементов
            swModel.ClearSelection();

            //вытянуть бобышку
            var feature = featureExtrusion(h.GetBodyLenght()/acc);
            swModel.ClearSelection();

            //получить грани бобышки
            var faces = feature.GetFaces();
            //выбрать вторую (вверх бобышки)
            var ent = faces[1] as Entity;
            //выбрать верхнюю грань
            ent.Select(true);
            //добавить на неё эскиз
            sm.InsertSketch(false);
            //*/

            //MessageBox.Show(h.GetBodyWidth().ToString());
            //MessageBox.Show(h.GetBodyHeight().ToString());
            //MessageBox.Show(h.GetBodyLenght().ToString());

            //MessageBox.Show(a.ToString());
            //MessageBox.Show(b.ToString());
            //MessageBox.Show(h.GetHoleWidth().ToString());
            //MessageBox.Show(h.GetHoleHeight().ToString());
            //MessageBox.Show(h.GetK().ToString());

            //holes
            double x_current = ((-h.GetBodyWidth()) /2.0 + (h.GetK() + h.GetHoleWidth() / 2.0)) /acc;
            double y_current = ((h.GetBodyHeight() / 2.0) - (h.GetK() + (h.GetHoleHeight() / 2.0))) / acc;
            double x_end = (((-h.GetBodyWidth()) / 2.0) + (h.GetK() + h.GetHoleWidth())) / acc;
            double y_end = ((h.GetBodyHeight() / 2.0) - (h.GetK() + h.GetHoleHeight())) / acc;
           
            double leftHoleCenterX = x_current;
            double leftHoleCenterY = y_current;

            double delta = (h.GetHoleWidth() + h.GetK()) / acc;

            int row, collumn;

            if (h.iterCheck())
            {
                row = (int)Math.Sqrt(Math.Pow(2, h.GetIterationNumber()));
                collumn = row;
            } else {
                row = (int)Math.Sqrt(Math.Pow(2, h.GetIterationNumber() - 1));
                collumn = 2 * row;
            }

            for (int i = 0; i < row; i++)
            {
                swModel.SketchManager.CreateCenterRectangle(x_current, y_current, 0, x_end, y_end, 0);
                for (int j = 1; j < collumn; j++)
                {
                    x_current = x_current + delta;
                    x_end = x_end + delta;
                    swModel.SketchManager.CreateCenterRectangle(x_current, y_current, 0, x_end, y_end, 0);
                }
                x_current = leftHoleCenterX;
                y_current = leftHoleCenterY - (h.GetK() + h.GetHoleHeight()) / acc;
                leftHoleCenterX = x_current;
                leftHoleCenterY = y_current;
                x_end = x_current + (h.GetHoleWidth() / 2.0) / acc;
                y_end = y_current + (h.GetHoleHeight() / 2.0) / acc;
            }

            featureCut(h.GetHoleLenght() / acc);
            swModel.ClearSelection();
            

            /*
            sm.CreateCenterRectangle(-2/acc, -2/acc, 0, -1 / acc, -1 / acc, 0);
            sm.CreateCenterRectangle(2 / acc, 2 / acc, 2 / acc, 0, 0, 0);

            featureCut(10/acc);
            swModel.ClearSelection();
            */
            /*
            sm.CreateCenterRectangle(-1 / acc, -1 / acc, -1 / acc, -a / 4, -b / 4, 0);
            featureCut(c, false, swEndConditions_e.swEndCondThroughAll);
            swModel.ClearSelection();
            */

            /*
            swModel.Extension.SelectByID2("Сверху", "PLANE", 0, 0, 0, false, 0, null, 0); //выбрал плоскость  
            swModel.SketchManager.InsertSketch(true); //вставил эскиз в режиме редактирования  
            swModel.SketchManager.CreateCenterRectangle(0, 0, 0, a / 2, b / 2, 0); // создал круг
            swModel.ClearSelection();

            var feature = swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, c, 0.01, false, false,
                false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false,
                true, true, true, 0, 0, false);

            //swModel.FeatureManager.FeatureCut3()

            feature.Select(false);
            feature.DeSelect();
            
            var faces = feature.GetFaces();
            var ent = faces[1] as Entity;
            ent.Select(true);
            /*swModel.SketchManager.CreateCircleByRadius(0.2, 0.2, 0.2, 0.1);
            ent.DeSelect();

            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.1, 0.01, false, false,
    false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false,
    true, true, true, 0, 0, false);
            */

            /*
            swModel.SketchManager.InsertSketch(true); //закрыл эскиз  
            swModel.ClearSelection2(true); //снял выделение с линии
            */

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
            return swModel.FeatureManager.FeatureCut2(true, flip, false, (int)mode, (int)mode,
                deepth, 0, false, false, false, false, 0, 0, false, false, false, false, false,
                false, false, false, false, false);
        }

        /// <summary>
        /// Вытянуть бобышку
        /// </summary>
        /// <param name="deepth">высота выдавливания</param>
        /// <param name="dir">направление выдвливания</param>
        /// <returns>объект бобышка</returns>
        private Feature featureExtrusion(double deepth, bool dir = false)
        {
            return swModel.FeatureManager.FeatureExtrusion2(true, false, dir,
                (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                deepth, 0, false, false, false, false, 0, 0, false, false, false, false, true,
                true, true, 0, 0, false);
        }

        private Boolean GetSolidworks()
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

            // Получает ISketchManager объект, который позволяет получить доступ к процедурам эскиза
            swSketchManager = (SketchManager)swModel.SketchManager;

            // Получает ISelectionMgr объект для данного документа, что делает выбранный объект доступным
            swSelMgr = (SelectionMgr)swModel.SelectionManager;


            /*// Проверка на открытие именно детали в SolidWorks
            if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
            {
                string text = "Это работает только на уровне чертежа";
                // Отображает окно сообщения с заданным текстом
                MessageBox.Show(text, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }*/

            return true;
        }

        private void RowNum_TextChanged(object sender, EventArgs e)
        {

            if (ColumnNum.Text == "" || RowNum.Text == "")
            {
                Iteration.Text = "";
                return;
            }
            else {
                //Добавить проверку на ввод чисел
                int a = Convert.ToInt32(RowNum.Text);
                int b = Convert.ToInt32(ColumnNum.Text);

                int i = 0;
                while (true)
                {
                    if(Math.Round(Math.Pow(2, i), 0) == a * b)
                    {
                        Iteration.Text = i.ToString();
                        return;
                    }
                    else if (Math.Round(Math.Pow(2, i), 0) > a * b)
                    {
                        Iteration.Text = "Не степень 2";
                        return;
                    }
                    i++;
                }

            }
        }



        //GOVNOKOD

        private void cmbBoxChooseDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVDDetailInfo.Clear();
            txtVDCSInfo.Clear();
            txtVDCellInfo.Clear();
            txtVDMaterialInfo.Clear();
            cmbVDChooseCell.Items.Clear();
            

            Detail detail = details.FindLast(x => x.Name == cmbBoxChooseDetail.SelectedItem.ToString());

            if (detail != null)
            {
                txtVDDetailInfo.Text = detail.GetStringNameAndDesc();

                txtVDDetailInfo.Text += detail.GetStringProperties();

                txtVDMaterialInfo.Text = detail.Material.ToString();

                txtVDCSInfo.Text = detail.CellStructure.Description + System.Environment.NewLine;
                txtVDCSInfo.Text += detail.CellStructure.GetStringProperties();

                foreach (Cell cell in detail.CellStructure.Cells)
                {
                    cmbVDChooseCell.Items.Add(cell.Name);
                }
            }
        }

        private void cmbVDChooseCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVDCellInfo.Clear();

            Detail detail = details.FindLast(x => x.Name.Equals(cmbBoxChooseDetail.SelectedItem.ToString()));

            Cell cell = detail.CellStructure.Cells.FindLast(x => x.Name.Equals(cmbVDChooseCell.SelectedItem.ToString()));

            txtVDCellInfo.Text = cell.ToString();
        }

        private bool IsNull(TextBox txt)
        {
            bool f = true;
            if ((txt.Text != null) && (!txt.Text.Equals("")))
            {
                f = false;
            }
            return f;
        }

        private bool IsNull(ComboBox cmb)
        {
            bool f = true;
            if ((cmb.SelectedItem != null) && (cmb.SelectedItem.ToString() != null) && (!cmb.SelectedItem.ToString().Equals("")))
            {
                f = false;
            }
            return f;
        }

        private bool IsNull(string txt)
        {
            bool f = true;
            if ((txt != null) && (!txt.Equals("")))
            {
                f = false;
            }
            return f;
        }


        private bool IsNumber(string str)
        {
            bool f = true;
            try
            {
                double numb = Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                f = false;
            }
            return f;
        }
        //add detail property
        private void btnADDetParamValue_Click(object sender, EventArgs e)
        {
            string txt = txtAVDetParamValue.Text;
            if ((IsNumber(txt)) && (!IsNull(cmbADDetParamChoose)))
            {
                string prop = cmbADDetParamChoose.SelectedItem.ToString();
                Property p = detProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                if (p != null)
                {
                    p.Value = new Value(Convert.ToDouble(txt));

                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("Property = 0");
                    } else
                    {

                        foreach (Property p1 in newDetail.Properties)
                        {
                            if (p1.Equals(p))
                            {
                                DialogManager.showDialogError("Property repit");
                                return;
                            }
                        }

                        newDetail.Properties.Add(p);
                        txtAdDetParams.Text = newDetail.GetStringProperties();
                    }

                    
                } else
                {
                    DialogManager.showDialogError("Property 404");
                }
            }
            else
            {
                DialogManager.showDialogError("Error value");
            }
        }
        //del det prop
        private void btnDelDetProp_Click(object sender, EventArgs e)
        {
            string txt = txtAVDetParamValue.Text;
            if (!IsNull(cmbADDetParamChoose))
            {
                string prop = cmbADDetParamChoose.SelectedItem.ToString();
                Property p = newDetail.Properties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                if (p != null)
                {

                    newDetail.Properties.Remove(p);

                    txtAdDetParams.Text = newDetail.GetStringProperties();
                }
                else
                {
                    DialogManager.showDialogError("Property 404");
                }
            }
            else
            {
                DialogManager.showDialogError("Chooce property");
            }
        }
        //add material
        private void btnSetMat_Click(object sender, EventArgs e)
        {          
            if (!IsNull(cmbADMatChoose))
            {
                string txt = cmbADMatChoose.SelectedItem.ToString();
                Material m = materials.FindLast(x => x.Name.Equals(txt));
                if (m != null)
                {
                    newDetail.Material = m;
                }
                else
                {
                    DialogManager.showDialogError("Material 404");
                }
                
            } else {
                DialogManager.showDialogError("choose Material");
            }
        }

        private void cmbADMatChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsNull(cmbADMatChoose))
            {
                Material m = materials.FindLast(x => x.Name.Equals(cmbADMatChoose.SelectedItem.ToString()));
                txtADMatInfo.Text = m.ToString();
            } else
            {
                DialogManager.showDialogError("choose Material");
            }
        }
        //add cs property
        private void btnADCSPropAdd_Click(object sender, EventArgs e)
        {
            string txt = txtCSPropValue.Text;
            if ((IsNumber(txt)) && (!IsNull(cmbCSProp)))
            {

                Property p = csProperties.FindLast(x => x.GetStringNameAndUnit().Equals(cmbCSProp.SelectedItem.ToString()));
                if (p != null)
                {
                    p.Value = new Value(Convert.ToDouble(txt));
                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("Property = 0");
                    }
                    else
                    {
                        foreach (Property p1 in newDetail.CellStructure.Properties)
                        {
                            if (p1.Equals(p))
                            {
                                DialogManager.showDialogError("Property repit");
                                return;
                            }
                        }

                        newDetail.CellStructure.Properties.Add(p);

                        txtADCSProp.Text = newDetail.CellStructure.GetStringProperties();
                    }

                } else {
                    DialogManager.showDialogError("Error 404");
                }            
            }
            else
            {
                DialogManager.showDialogError("Error value");
            }
        }

        //del cs prop
        private void btnDelCSProp_Click(object sender, EventArgs e)
        {
            if ((!IsNull(cmbCSProp)))
            {

                Property p = newDetail.CellStructure.Properties.FindLast(x => x.GetStringNameAndUnit().Equals(cmbCSProp.SelectedItem.ToString()));
                if (p != null)
                {

                    newDetail.CellStructure.Properties.Remove(p);
                    txtADCSProp.Text = newDetail.CellStructure.GetStringProperties();
                }
                else
                {
                    DialogManager.showDialogError("Property 404");
                }
            }
            else
            {
                DialogManager.showDialogError("Chooce property");
            }
        }

        //view choose cell
        private void cmbADCellChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsNull(cmbADCellChoose))
            {
                string txt = cmbADCellChoose.SelectedItem.ToString();
                Cell c = sqlWorker.GetCells().FindLast(x => x.Name.Equals(txt));
                txtADCellInfo.Text = c.ToString();
            }
        }        
        //choose material
        private void chooseMaterial_CheckedChanged(object sender, EventArgs e)
        {
            objProperties.Clear();
            loadMaterialProperty(cmbADAddProp);

            txtADAddName.Clear();
            txtADAddDesc.Clear();

            gbCT.Hide();

            printListProperties(txtADAddProp, objProperties);
            btnADAddObj.Text = "Add Material";

        }
        //view objProp
        private void printListProperties(TextBox txt, List<Property> list)
        {
            txt.Clear();
            txt.Text = "Propperties:" + System.Environment.NewLine;
            foreach (Property p in list)
            {
                txt.Text += p + System.Environment.NewLine;
            }
        }
        //choose cell
        private void chooseCell_CheckedChanged(object sender, EventArgs e)
        {
            objProperties.Clear() ;

            loadCellProperty(cmbADAddProp);

            btnADAddObj.Text = "Add Cell";
            txtADAddName.Clear();
            txtADAddDesc.Clear();

            gbCT.Show();

            printListProperties(txtADAddProp, objProperties);            


            loadCellTypes();


        }

        //add material or cell
        //comment EMPTY MATERIAL PROPERTIES
        private void btnADAddObj_Click(object sender, EventArgs e)
        {

            if (!IsNull(txtADAddName) && !IsNull(txtADAddDesc))
            {
                if (chooseMaterial.Checked)
                {
                    Material material = new Material();

                    //if (objProperties.Count == 0)
                    //{
                    //    DialogManager.showDialogError("Empty properties");
                    //    return;
                    //}

                    material.Name = txtADAddName.Text;
                    material.Description = txtADAddDesc.Text;
                    material.Properties = new List<Property>(objProperties);
                    foreach (Material mat in materials)
                    {
                        if (mat.Equals(material))
                        {
                            DialogManager.showDialogError("Repit detail");
                            return;
                        }
                    }

                    objProperties.Clear();
                    printListProperties(txtADAddProp, objProperties);

                    if (sqlWorker.AddMaterial(material))
                    {
                        loadMaterials();
                    } else
                    {
                        DialogManager.showDialogError("DB error");
                    }          
                }
                else
                {
                    Cell cell = new Cell();

                    cell.Name = txtADAddName.Text;
                    cell.Description = txtADAddDesc.Text;

                    //if (objProperties.Count == 0)
                    //{
                    //    DialogManager.showDialogError("Empty properties");
                    //    return;
                    //}

                    cell.Properties = new List<Property>(objProperties);                  

                    if (!IsNull(cmbADCellTypes))
                    {
                        string txt = cmbADCellTypes.SelectedItem.ToString();
                        cell.CellType = sqlWorker.GetCellTytes().FindLast(x => x.TypeName.Equals(txt));
                    } else
                    {
                        DialogManager.showDialogError("Empty Cell Type");
                        return;
                    }

                    List<Cell> list = sqlWorker.GetCells();

                    foreach (Cell c in list)
                    {
                        if (c.Name.Equals(cell.Name))
                        {
                            DialogManager.showDialogError("Repit detail");
                            return;
                        }
                    }

                    objProperties.Clear();
                    printListProperties(txtADAddProp, objProperties);

                    if (sqlWorker.AddCell(cell))
                    {
                        loadCells();
                    }
                    else
                    {
                        DialogManager.showDialogError("DB error");
                    }
                }
            } else
            {
                DialogManager.showDialogError("Empty Field");
            }
                
        }

        //add obj Prop
        private void btnAddProp_Click(object sender, EventArgs e)
        {
            string value = txtADAddValue.Text;         
            if ((IsNumber(value)) && (!IsNull(cmbADAddProp)))
            {
                string prop = cmbADAddProp.SelectedItem.ToString();
                Models.Property p;
                if (chooseMaterial.Checked)
                {
                    p = matProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                }
                else {
                    p = detProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                }
                if (p!=null)
                {
                    p.Value = new Value(Convert.ToDouble(value));

                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("Property = 0");
                        return;
                    } 

                    foreach (Property p1 in objProperties)
                    {
                        if (p1.Equals(p))
                        {
                            DialogManager.showDialogError("Property repit");
                            return;
                        }
                    }

                    objProperties.Add(p);

                    printListProperties(txtADAddProp, objProperties);
                }
                else
                {
                    DialogManager.showDialogError("Property not found");
                }
            }
            else
            {
                DialogManager.showDialogError("Error value");
            }
        }
        //del obj prop
        private void btnDelObjProp_Click(object sender, EventArgs e)
        {
            if ((!IsNull(cmbADAddProp)))
            {

                Property p = objProperties.FindLast(x => x.GetStringNameAndUnit().Equals(cmbADAddProp.SelectedItem.ToString()));
                if (p != null)
                {

                    objProperties.Remove(p);
                    printListProperties(txtADAddProp, objProperties);
                }
                else
                {
                    DialogManager.showDialogError("Property 404");
                }
            }
            else
            {
                DialogManager.showDialogError("Chooce property");
            }
        }

        //clear all fields
        private void ClearAddDetailPage()
        {
            txtADDetDesc.Clear();
            txtADDetName.Clear();
            txtAdDetParams.Clear();
            
            txtAVDetParamValue.Clear();

            txtADMatInfo.Clear();

            txtADCSProp.Clear();
            cmbCSProp.Items.Clear();

            txtCSPropValue.Clear();
            txtADCSCells.Clear();
            txtADCellInfo.Clear();

            cmbADCellChoose.Items.Clear();

            txtADAddProp.Clear();

            cmbADAddProp.Items.Clear();

            txtADAddValue.Clear();
            txtADAddDesc.Clear();
            txtADAddName.Clear();

            chooseMaterial.Checked = true;
            chooseCell.Checked = false;

    }

        // add detail
        private void btnAddDetail_Click(object sender, EventArgs e)
        {

            if (!IsNull(txtADDetName))
            {
                newDetail.Name = txtADDetName.Text;
            }
            else
            {
                DialogManager.showDialogError("Empty Name Detail");
                return;
            }
            if (!IsNull(txtADDetDesc))
            {
                newDetail.Description = txtADDetDesc.Text;
            }
            else
            {
                DialogManager.showDialogError("Empty Description Detail");
                return;
            }

            if (!IsNull(txtADCSDesc))
            {
                newDetail.CellStructure.Description = txtADCSDesc.Text;
            }
            else
            {
                DialogManager.showDialogError("Empty Description CS");
                return;
            }

            if (!IsNull(cmbADMatChoose))
            {
                newDetail.Material = materials.FindLast(x => x.Name.Equals(cmbADMatChoose.SelectedItem.ToString()));
            }
            else
            {
                DialogManager.showDialogError("Empty Material Detail");
                return;
            }

            //if (newDetail.Properties.Count == 0)
            //{
            //    DialogManager.showDialogError("Empty Detail Property");
            //    return;
            //}

            //if (newDetail.CellStructure.Properties.Count == 0)
            //{
            //    DialogManager.showDialogError("Empty CS Property");
            //    return;
            //}

            //if (newDetail.CellStructure.Cells.Count == 0)
            //{
            //    DialogManager.showDialogError("Empty Cells");
            //    return;
            //}

            if (newDetail != null)
            { 
                foreach (Detail d in details)
                {
                    if (d.Equals(newDetail))
                    {
                        DialogManager.showDialogError("Repit detail");
                        return;
                    }
                }

                if (sqlWorker.AddDetail(newDetail))
                {
                    DialogManager.showDialogInfo("Sucsess");
                    newDetail = new Detail();
                } else
                {
                    DialogManager.showDialogError("Error db");
                }
                loadDetails();
                

            }
        }

        // add cell to cs
        private void btnADAddCell_Click(object sender, EventArgs e)
        {
            if (!IsNull(cmbADCellChoose))
            {
                Cell cell = sqlWorker.GetCells().FindLast(x => x.Name.Equals(cmbADCellChoose.SelectedItem.ToString()));

                foreach (Cell c in newDetail.CellStructure.Cells)
                {
                    if (c.Equals(cell))
                    {
                        DialogManager.showDialogError("Repeat cell");
                        return;
                    }
                }
                newDetail.CellStructure.Cells.Add(cell);

                txtADCSCells.Text = newDetail.CellStructure.GetStringCells();
            }
            else
            {
                DialogManager.showDialogError("Error value");
            }

        }  

        // add ct to bd
        private void btnAddCT_Click(object sender, EventArgs e)
        {
            if (!IsNull(txtCellType))
            {
                CellType ct = new CellType();
                ct.TypeName = txtCellType.Text;

                List<CellType> list = sqlWorker.GetCellTytes();
                foreach(CellType cet in list)
                {
                    if (cet.Equals(ct))
                    {
                        DialogManager.showDialogError("Repeat cell type");
                        return;
                    }
                }

                if (sqlWorker.AddCellType(ct))
                {
                    DialogManager.showDialogInfo("Успех"+ System.Environment.NewLine+"Используйте comboBox для set");
                    loadCellTypes();
                } else
                {
                    DialogManager.showDialogError("Try again");
                }

            } else
            {
                DialogManager.showDialogError("Enter type name");
            }
        }

        // del cell from cs
        private void btnDelCell_Click(object sender, EventArgs e)
        {
            if (!IsNull(cmbADCellChoose))
            {
                Cell cell = sqlWorker.GetCells().FindLast(x => x.Name.Equals(cmbADCellChoose.SelectedItem.ToString()));

                foreach (Cell c in newDetail.CellStructure.Cells)
                {
                    if (c.Equals(cell))
                    {
                        newDetail.CellStructure.Cells.Remove(c);
                        break;
                    }
                }                
                txtADCSCells.Text = newDetail.CellStructure.GetStringCells();
            }
            else
            {
                DialogManager.showDialogError("Error value");
            }
        }
        //add new property
        private void btnAddNewProp_Click(object sender, EventArgs e)
        {
            if (!IsNull(txtNewPropName) && !IsNull(cmbChooseParentType))
            {
                if (IsNull(txtNewPropUnit))
                {
                    if (!DialogManager.showDialogYesNo("Empty Unit. Continue?"))
                    {
                        return;
                    }
                }   
                    string txt = cmbChooseParentType.SelectedItem.ToString();
                    


                    Property p = new Property(txtNewPropName.Text, txtNewPropUnit.Text);


                    if (ConvertToParentType(txt)==null)
                    {
                        DialogManager.showDialogError("Empty fields");
                        return;
                    }
                    ParentType pt = (ParentType)ConvertToParentType(txt);                  

                    switch (pt)
                    {
                        case ParentType.Detail:
                            foreach (Property prop in detProperties)
                            {
                                if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                {
                                    DialogManager.showDialogError("Repeat property");
                                    return;
                                }
                            }
                            sqlWorker.AddProperty(p, pt);
                            loadDetailProperty(cmbADDetParamChoose);
                            break;
                        case ParentType.Material:
                            if (chooseMaterial.Checked)
                            {
                                loadMaterialProperty(cmbADAddProp);
                                foreach (Property prop in detProperties)
                                {
                                    if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                    {
                                        DialogManager.showDialogError("Repeat property");
                                        return;
                                    }
                                }
                                sqlWorker.AddProperty(p, pt);
                            }
                            break;
                        case ParentType.Cell:
                            if (chooseCell.Checked)
                            {
                                loadCellProperty(cmbADAddProp);
                                foreach (Property prop in detProperties)
                                {
                                    if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                    {
                                        DialogManager.showDialogError("Repeat property");
                                        return;
                                    }
                                }
                                sqlWorker.AddProperty(p, pt);
                            }
                            break;
                        case ParentType.CelluralStructure:
                            foreach (Property prop in detProperties)
                            {
                                if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                { 
                                DialogManager.showDialogError("Repeat property");
                                    return;
                                }
                            }
                            sqlWorker.AddProperty(p, pt);
                            loadCSProperty(cmbCSProp);
                            break;
                    }

                

            } else
            {
                DialogManager.showDialogError("Empty fields");
            }
        }

        private Object ConvertToParentType(String str)
        {
            foreach (ParentType pt in Enum.GetValues(typeof(ParentType)))
            {
                if (str.Equals(pt.ToString()))
                {
                    return pt;
                }
            }
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();

            f.Close();
        }
    }
}
