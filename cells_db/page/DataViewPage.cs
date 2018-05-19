using cells_db.model;
using cells_db.sql;
using cells_db.util;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Cube
{
    internal partial class DataViewPage : Form
    {
        private ISQLWorker sqlWorker;

        private Detail newDetail = new Detail();
        private List<Property> objProperties = new List<Property>();

        private Data data = new Data();

        //Add Detail
        //List<Cell> cells;
        

        public DataViewPage(ISQLWorker SQLWorker)
        {
            sqlWorker = SQLWorker;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            data.loadDetProperties(sqlWorker);
            cmb.Items.Clear();
            foreach (Property p in data.detProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadCSProperty(ComboBox cmb)
        {
            data.loadCSProperties(sqlWorker);
            cmb.Items.Clear();
            foreach (Property p in data.csProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadCellProperty(ComboBox cmb)
        {
            data.loadCellProperties(sqlWorker);
            cmb.Items.Clear();
            foreach (Property p in data.cellProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }

        private void loadMaterialProperty(ComboBox cmb)
        {
            data.loadMatProperties(sqlWorker);
            cmb.Items.Clear();
            foreach (Property p in data.matProperties)
            {
                cmb.Items.Add(p.GetStringNameAndUnit());
            }
        }



        private void loadDetails()
        {
            data.loadDetails(sqlWorker);
            foreach (Detail d in data.details)
            {
                cmbBoxChooseDetail.Items.Add(d.Name);
            }
        }

        private void loadMaterials()
        {
            cmbADMatChoose.Items.Clear();
            data.loadMaterials(sqlWorker);
            foreach (Material m in data.materials)
            {
                cmbADMatChoose.Items.Add(m.Name);
            }
        }
        public void loadCells()
        {
            cmbADCellChoose.Items.Clear();

            data.loadCells(sqlWorker);
            foreach (Cell c in data.cells)
            {
                cmbADCellChoose.Items.Add(c.Name);
            }
        }

        public void loadCellTypes()
        {
            cmbADCellTypes.Items.Clear();
            data.loadCellTypes(sqlWorker);
            foreach (CellType ct in data.cellTypes)
            {
                cmbADCellTypes.Items.Add(ct.TypeName);
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
            ResultsTxt.Clear();

            Detail detail = data.details.FindLast(x => x.Name == cmbBoxChooseDetail.SelectedItem.ToString());

            if (detail != null)
            {
                txtVDDetailInfo.Text = detail.GetStringNameAndDesc();

                txtVDDetailInfo.Text += detail.GetStringProperties();

                txtVDMaterialInfo.Text = detail.Material.ToString();

                txtVDCSInfo.Text = detail.CellStructure.Description + Environment.NewLine;
                txtVDCSInfo.Text += detail.CellStructure.GetStringProperties();

                ResultsTxt.Text = detail.GetStringResults();

                foreach (Cell cell in detail.CellStructure.Cells)
                {
                    cmbVDChooseCell.Items.Add(cell.Name);
                }
            }
        }

        private void cmbVDChooseCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVDCellInfo.Clear();

            Detail detail = data.details.FindLast(x => x.Name.Equals(cmbBoxChooseDetail.SelectedItem.ToString()));

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
                Property p = data.detProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                if (p != null)
                {
                    p.Value = new Value(Convert.ToDouble(txt));

                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("The property can not be 0.");
                    } else
                    {

                        foreach (Property p1 in newDetail.Properties)
                        {
                            if (p1.Equals(p))
                            {
                                DialogManager.showDialogError("The property can not be repeated.");
                                return;
                            }
                        }

                        newDetail.Properties.Add(p);
                        txtAdDetParams.Text = newDetail.GetStringProperties();
                    }

                    
                } else
                {
                    DialogManager.showDialogError("Property not found.");
                }
            }
            else
            {
                DialogManager.showDialogError("Please enter correct information.");
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
                    DialogManager.showDialogError("Property not found.");
                }
            }
            else
            {
                DialogManager.showDialogError("Select property please.");
            }
        }
        //add material
        private void btnSetMat_Click(object sender, EventArgs e)
        {          
            if (!IsNull(cmbADMatChoose))
            {
                string txt = cmbADMatChoose.SelectedItem.ToString();
                Material m = data.materials.FindLast(x => x.Name.Equals(txt));
                if (m != null)
                {
                    newDetail.Material = m;
                }
                else
                {
                    DialogManager.showDialogError("Material not found.");
                }
                
            } else {
                DialogManager.showDialogError("Select Material please.");
            }
        }

        private void cmbADMatChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsNull(cmbADMatChoose))
            {
                Material m = data.materials.FindLast(x => x.Name.Equals(cmbADMatChoose.SelectedItem.ToString()));
                txtADMatInfo.Text = m.ToString();
            } else
            {
                DialogManager.showDialogError("Select Material please.");
            }
        }
        //add cs property
        private void btnADCSPropAdd_Click(object sender, EventArgs e)
        {
            string txt = txtCSPropValue.Text;
            if ((IsNumber(txt)) && (!IsNull(cmbCSProp)))
            {

                Property p = data.csProperties.FindLast(x => x.GetStringNameAndUnit().Equals(cmbCSProp.SelectedItem.ToString()));
                if (p != null)
                {
                    p.Value = new Value(Convert.ToDouble(txt));
                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("The property can not be 0.");
                    }
                    else
                    {
                        foreach (Property p1 in newDetail.CellStructure.Properties)
                        {
                            if (p1.Equals(p))
                            {
                                DialogManager.showDialogError("The property can not be repeated.");
                                return;
                            }
                        }

                        newDetail.CellStructure.Properties.Add(p);

                        txtADCSProp.Text = newDetail.CellStructure.GetStringProperties();
                    }

                } else {
                    DialogManager.showDialogError("Property not found.");
                }            
            }
            else
            {
                DialogManager.showDialogError("You must enter a number.");
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
                    DialogManager.showDialogError("Property not found.");
                }
            }
            else
            {
                DialogManager.showDialogError("Select property please.");
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
            txt.Text = "Properties:" + System.Environment.NewLine;
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
                    foreach (Material mat in data.materials)
                    {
                        if (mat.Equals(material))
                        {
                            DialogManager.showDialogError("The material can not be repeated.");
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
                        DialogManager.showDialogError("Database Error.");
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
                        cell.CellType = sqlWorker.GetCellTypes().FindLast(x => x.TypeName.Equals(txt));
                    } else
                    {
                        DialogManager.showDialogError("Select Cell Type please.");
                        return;
                    }

                    List<Cell> list = sqlWorker.GetCells();

                    foreach (Cell c in list)
                    {
                        if (c.Name.Equals(cell.Name))
                        {
                            DialogManager.showDialogError("The cell can not be repeated.");
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
                        DialogManager.showDialogError("Database Error");
                    }
                }
            } else
            {
                DialogManager.showDialogError("Please enter correct information.");
            }
                
        }

        //add obj Prop
        private void btnAddProp_Click(object sender, EventArgs e)
        {
            string value = txtADAddValue.Text;         
            if ((IsNumber(value)) && (!IsNull(cmbADAddProp)))
            {
                string prop = cmbADAddProp.SelectedItem.ToString();
                Property p;
                if (chooseMaterial.Checked)
                {
                    p = data.matProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                }
                else {
                    p = data.detProperties.FindLast(x => x.GetStringNameAndUnit().Equals(prop));
                }
                if (p!=null)
                {
                    p.Value = new Value(Convert.ToDouble(value));

                    if (p.Value.value == 0)
                    {
                        DialogManager.showDialogError("The Property can not be 0.");
                        return;
                    } 

                    foreach (Property p1 in objProperties)
                    {
                        if (p1.Equals(p))
                        {
                            DialogManager.showDialogError("The Property can not be repeated.");
                            return;
                        }
                    }

                    objProperties.Add(p);

                    printListProperties(txtADAddProp, objProperties);
                }
                else
                {
                    DialogManager.showDialogError("Property not found.");
                }
            }
            else
            {
                DialogManager.showDialogError("Please enter correct information.");
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
                    DialogManager.showDialogError("Property not found.");
                }
            }
            else
            {
                DialogManager.showDialogError("Select property please");
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
                DialogManager.showDialogError("Please Enter the Name Detail.");
                return;
            }
            if (!IsNull(txtADDetDesc))
            {
                newDetail.Description = txtADDetDesc.Text;
            }
            else
            {
                DialogManager.showDialogError("Please Enter the Description Detail.");
                return;
            }

            if (!IsNull(txtADCSDesc))
            {
                newDetail.CellStructure.Description = txtADCSDesc.Text;
            }
            else
            {
                DialogManager.showDialogError("Please Enter the Description CellStructure.");
                return;
            }

            if (!IsNull(cmbADMatChoose))
            {
                newDetail.Material = data.materials.FindLast(x => x.Name.Equals(cmbADMatChoose.SelectedItem.ToString()));
            }
            else
            {
                DialogManager.showDialogError("Please set the material.");
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
                foreach (Detail d in data.details)
                {
                    if (d.Equals(newDetail))
                    {
                        DialogManager.showDialogError("The detail can not be repeated.");
                        return;
                    }
                }

                if (sqlWorker.AddDetail(newDetail))
                {
                    DialogManager.showDialogInfo("Sucsess!");
                    newDetail = new Detail();
                } else
                {
                    DialogManager.showDialogError("DataBase Error.");
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
                        DialogManager.showDialogError("The cell can not be repeated.");
                        return;
                    }
                }
                newDetail.CellStructure.Cells.Add(cell);

                txtADCSCells.Text = newDetail.CellStructure.GetStringCells();
            }
            else
            {
                DialogManager.showDialogError("Please enter correct information.");
            }

        }  

        // add ct to bd
        private void btnAddCT_Click(object sender, EventArgs e)
        {
            if (!IsNull(txtCellType))
            {
                CellType ct = new CellType();
                ct.TypeName = txtCellType.Text;

                List<CellType> list = sqlWorker.GetCellTypes();
                foreach(CellType cet in list)
                {
                    if (cet.Equals(ct))
                    {
                        DialogManager.showDialogError("The cell type can not be repeated.");
                        return;
                    }
                }

                if (sqlWorker.AddCellType(ct))
                {                
                    loadCellTypes();
                } else
                {
                    DialogManager.showDialogError("DataBase error. Try again.");
                }

            } else
            {
                DialogManager.showDialogError("Please Enter cell type Name.");
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
                DialogManager.showDialogError("Please enter correct information.");
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
                        DialogManager.showDialogError("Empty fields.");
                        return;
                    }
                    ParentType pt = (ParentType)ConvertToParentType(txt);                  

                    switch (pt)
                    {
                        case ParentType.Detail:
                            foreach (Property prop in data.detProperties)
                            {
                                if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                {
                                    DialogManager.showDialogError("The property can not be repeated.");
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
                                foreach (Property prop in data.detProperties)
                                {
                                    if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                    {
                                        DialogManager.showDialogError("The property can not be repeated.");
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
                                foreach (Property prop in data.detProperties)
                                {
                                    if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                    {
                                        DialogManager.showDialogError("The property can not be repeated.");
                                        return;
                                    }
                                }
                                sqlWorker.AddProperty(p, pt);
                            }
                            break;
                        case ParentType.CelluralStructure:
                            foreach (Property prop in data.detProperties)
                            {
                                if (p.GetStringNameAndUnit().Equals(prop.GetStringNameAndUnit()))
                                { 
                                DialogManager.showDialogError("The property can not be repeated.");
                                    return;
                                }
                            }
                            sqlWorker.AddProperty(p, pt);
                            loadCSProperty(cmbCSProp);
                            break;
                    }

                

            } else
            {
                DialogManager.showDialogError("Empty fields.");
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

    }
}
