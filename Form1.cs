using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using cells_db.model;
using cells_db.sql;
using cells_db.util;
using cells_db.exception;
using cells_db.model;

namespace Cube
{
    public partial class Form1 : Form
    {
        SLDManager application;

        BodyParam body;
        BodyDrawer bodyDrawer;

        Cells cells;
        CellsDrawer cellsDrawer;

        Research research;
        Research researchLoop;

        List<object[]> action;

        List<object[]> dataStress;
        List<object[]> dataDisplacement;

        public Form1()
        {
            InitializeComponent();
            deleteBody.Visible = false;
            possibilities.Visible = false;
            deleteCellsButton2.Visible = false;

            label16.Visible = false; bodyPartsComboBox3.Visible = false;
            createWeb.Visible = false; material.Visible = false;
            groupBox5.Visible = false;
            researchBodyButton3.Visible = true; startResearch.Visible = false; deleteResearch3.Visible = false;
            diagramButton.Visible = false;

            cubeRadioButton_CheckedChanged(null, null);
            iterationRadioButton1_CheckedChanged(null, null);
            radioButton8_CheckedChanged(null, null);
            multiplierCheck_CheckedChanged(null, null);
            checkBox1_CheckedChanged(null, null);
            radioButton5_CheckedChanged(null, null);
            FixRadioButton_CheckedChanged_1(null, null);
            radioButton11_CheckedChanged(null, null);
        }
        
        private void drawBody_Click(object sender, EventArgs e)
        {
            cubeRadioButton.Enabled = false; paralRadioButton.Enabled = false;
            drawBody.Visible = false; deleteBody.Visible = true;
            checkUseSolid.Enabled = false;
            possibilities.Visible = true;

            if (checkUseSolid.Checked) {
                possibilities.TabPages[1].Enabled = true;
                possibilities.TabPages[2].Enabled = true;
                possibilities.TabPages[3].Enabled = true;
            }
            else
            {
                possibilities.TabPages[1].Enabled = false;
                possibilities.TabPages[2].Enabled = false;
                possibilities.TabPages[3].Enabled = false;
            }

            //заблокировать изменение величин
            if (cubeRadioButton.Checked)
            {
                bodyWidthText.Enabled = false;
            }

            if (paralRadioButton.Checked)
            {
                bodyWidthText.Enabled = false;
                bodyHeightText.Enabled = false;
                bodyLenghtText.Enabled = false;
            }
            
            //рисование тела
            double bodyWidth, bodyHeight, bodyLenght;

            if (cubeRadioButton.Checked)
            {
                bodyWidth = Convert.ToDouble(bodyWidthText.Text.Replace('.', ','));
                bodyHeight = bodyWidth; bodyLenght = bodyWidth;
            }
            else
            {
                bodyWidth = Convert.ToDouble(bodyWidthText.Text.Replace('.', ','));
                bodyHeight = Convert.ToDouble(bodyHeightText.Text.Replace('.', ','));
                bodyLenght = Convert.ToDouble(bodyLenghtText.Text.Replace('.', ','));
            }

            body = new BodyParam(bodyWidth, bodyHeight, bodyLenght);

            if (checkUseSolid.Checked)
            {
                //Запустить рисование тела ... добавить проверки
                application = new SLDManager(); application.GetSolidworks();

                bodyDrawer = new BodyDrawer(application, body);
                bodyDrawer.drawBody();
                body.SetFaces(bodyDrawer.GetFacesArray());
            }
            else
            {
                bodyDrawer = null; application = null;
            }

          
        }

        private void deleteBody_Click(object sender, EventArgs e)
        {
            cubeRadioButton.Enabled = true; paralRadioButton.Enabled = true;
            drawBody.Visible = true; deleteBody.Visible = false;
            checkUseSolid.Enabled = true;
            possibilities.Visible = false;

            if(cellsDrawer != null){
                cellsDrawer.deleteCells();
            }

            //разблокировать изменение величин
            if (cubeRadioButton.Checked)
            {
                bodyWidthText.Enabled = true;
            }

            if (paralRadioButton.Checked)
            {
                bodyWidthText.Enabled = true;
                bodyHeightText.Enabled = true;
                bodyLenghtText.Enabled = true;
            }

            //удаление тела
            bodyDrawer.deleteBody();
            bodyDrawer = null; body = null;
        }

        private void calculateButton1_Click(object sender, EventArgs e)
        {
            resultTextBox1.Clear();
            preparationCalculation(accuracyTextBox1, iterationTextBox1, multiplierRadioButton1, 
                multiplierTextBox1, denominatorTextBox1);


            if (loopRadioButton1.Checked)
            {
                for (int i = cells.GetIterationNumber(); i <= Convert.ToInt16(rowTextBox1.Text); i++)
                {
                    cells.SetIterationNumber(i);
                    cells.KCalculation();
                    textResults(resultTextBox1);
                }
            }
            else
            {
                cells.KCalculation();
                textResults(resultTextBox1);
            }
        }

        private void preparationCalculation(TextBox accuracy, TextBox iterationTextBox, RadioButton multiplierRadioButton, 
            TextBox multiplierTextBox, TextBox denominatorTextBox)
        {
            cells = new Cells(body);
            cells.SetIterationNumber((Convert.ToInt16(iterationTextBox.Text)));
            cells.SetAccuracy(Convert.ToInt16(accuracy.Text));

            if (multiplierRadioButton.Checked)
            {
                var buff = Convert.ToDouble(multiplierTextBox.Text.Replace('.', ','));
                if (buff >= 1)
                {
                    MessageBox.Show("Множитель не может быть больше 1");
                    return;
                }
                cells.SetCellsVWithMultiplier(buff);
            }
            else
            {
                if (Convert.ToDouble(multiplierTextBox.Text.Replace('.', ',')) >= 
                    Convert.ToDouble(denominatorTextBox.Text.Replace('.', ',')))
                {
                    MessageBox.Show("Числитель не может быть больше знаменателя");
                    return;
                }
                cells.SetCellsVWithMultiplier(Convert.ToDouble(multiplierTextBox.Text.Replace('.', ',')),
                    Convert.ToDouble(denominatorTextBox.Text.Replace('.', ',')));
            }

            cells.SetCellsNum();

        }

        private void textResults(TextBox box)
        {
            box.Text += "Номер итерации - " + cells.GetIterationNumber().ToString();
            box.Text += Environment.NewLine;
            box.Text += "Количество ячеек - " + (cells.CellsInRowNumber() * cells.CellsInColumnNumber()).ToString();
            box.Text += Environment.NewLine;
            box.Text += "Часть объёма, занимаемая ячеками - " + cells.GetCellsV().ToString();
            box.Text += Environment.NewLine;
            box.Text += "ФАКТИЧЕСКАЯ часть объёма, занимаемая ячеками - " + cells.GetVCellsFactical().ToString();
            box.Text += Environment.NewLine;
            box.Text += "K - " + cells.GetK().ToString();
            box.Text += Environment.NewLine;
            box.Text += "Ширина 1 ячейки - " + cells.GetHoleWidth().ToString();
            box.Text += Environment.NewLine;
            box.Text += "Высота 1 ячейки - " + cells.GetHoleHeight().ToString();
            box.Text += Environment.NewLine;
            box.Text += "Длинна 1 ячейки - " + cells.GetHoleLenght().ToString();
            box.Text += Environment.NewLine;
            box.Text += Environment.NewLine;
        }

        private void calculateButton2_Click(object sender, EventArgs e)
        {
            resultTextBox2.Clear();
            preparationCalculation(accuracyTextBox2, iterationTextBox2, multiplierRadioButton2,
                multiplierTextBox2, denominatorTextBox2);

            if (loopRadioButton2.Checked)
            {
                for (int i = cells.GetIterationNumber(); i <= Convert.ToInt16(rowTextBox2.Text); i++)
                {
                    cells.SetIterationNumber(i);
                    cells.KCalculation();
                    textResults(resultTextBox2);
                }
            }
            else
            {
                cells.KCalculation();
                textResults(resultTextBox2);
            }
        }

        private void drawCellsButton2_Click(object sender, EventArgs e)
        {
            drawCellsButton2.Visible = false;
            deleteCellsButton2.Visible = true;

            preparationCalculation(accuracyTextBox2, iterationTextBox2, multiplierRadioButton2,
                multiplierTextBox2, denominatorTextBox2);
            

            cellsDrawer = new CellsDrawer(application, body, bodyDrawer);
            

            if (loopRadioButton2.Checked)
            {
                int start, end, step;
                start = Convert.ToInt16(iterationTextBox2.Text);
                end = Convert.ToInt16(rowTextBox2.Text);
                //Проверка шага
                if (columnTextBox2.Text == "" || columnTextBox2.Text == "0" || Convert.ToInt16(columnTextBox2.Text) < 0)
                {
                    //шаг по умолчанию ( 1 )
                    step = 1;
                }
                else
                { //заданный шаг
                    step = Convert.ToInt16(columnTextBox2.Text);
                }

                for(int i = start; i <= end; i=i+step)
                {
                    cells.SetIterationNumber(i);
                    cells.KCalculation();
                    cellsDrawer.SetCells(cells);
                    cellsDrawer.drawCells();
                    //задержка
                    Thread.Sleep(2500);
                    if (i != end) { cellsDrawer.deleteCells(); }
                }
            }
            else {
                cells.KCalculation();
                cellsDrawer.SetCells(cells); cellsDrawer.drawCells();
            }
  
        }

        private void deleteCellsButton2_Click(object sender, EventArgs e)
        {
            deleteCellsButton2.Visible = false;
            drawCellsButton2.Visible = true;

            cellsDrawer.deleteCells();
            cellsDrawer = null;
        }

        private void researchBodyButton3_Click(object sender, EventArgs e)
        {
            label16.Visible = true; bodyPartsComboBox3.Visible = true;
            createWeb.Visible = true; material.Visible = true;
            groupBox5.Visible = true;
            researchBodyButton3.Visible = false; startResearch.Visible = true; deleteResearch3.Visible = true;

            research = new Research(application, body.GetFaces());
            research.CreateStudy();
            bodyPartsComboBox3.Items.Clear();

            for (int i = 0; i < body.GetFaces().Length; i++)
            {
                bodyPartsComboBox3.Items.Add(String.Join(", ", body.GetFaces().GetValue(i).ToString()));
            }
        }

        private void accept_Click(object sender, EventArgs e)
        {
            research.BodyParts_Select(bodyPartsComboBox3.SelectedIndex);

            if (fixRadioButton3.Checked){  research.FixFace(); }
            else { research.CreateLoad(Convert.ToDouble(pressureTextBox3.Text.Replace('.', ','))); }
        }


        private void startResearch_Click(object sender, EventArgs e)
        {
            startResearch.Visible = false; deleteResearch3.Visible = true;

            research.RunAnalysis();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label16.Visible = false; bodyPartsComboBox3.Visible = false;
            createWeb.Visible = false; material.Visible = false;
            groupBox5.Visible = false;
            researchBodyButton3.Visible = true; startResearch.Visible = false; deleteResearch3.Visible = false;

            research.deleteStudy(); research = null;
        }

        private void createWeb_Click(object sender, EventArgs e) { research.CreateMesh(); }

        private void material_Click(object sender, EventArgs e) {  research.MaterialSet(); }

        private void button6_Click(object sender, EventArgs e)
        {
            startResearch4.Visible = false; deleteResearch4.Visible = true;
            diagramButton.Visible = true;

            dataStress = null;  dataStress = new List<object[]>();
            dataDisplacement = null; dataDisplacement = new List<object[]>();

            if (!loopRadioButton2.Checked) {
                MessageBox.Show("Для циклического исследования необходимо выбрать \"Циклическое построение\""+
                    " в разделе \"Вычисление и конструирование\"");
                return;
            }

            // подготовка объекта cells
            preparationCalculation(accuracyTextBox2, iterationTextBox2, multiplierRadioButton2,
                multiplierTextBox2, denominatorTextBox2);

            cellsDrawer = new CellsDrawer(application, body, bodyDrawer);

            int start, end, step;
            start = Convert.ToInt16(iterationTextBox2.Text); end = Convert.ToInt16(rowTextBox2.Text);
            //Проверка шага
            if (columnTextBox2.Text == "" || columnTextBox2.Text == "0" || Convert.ToInt16(columnTextBox2.Text) < 0)
            { //шаг по умолчанию ( 1 )
                step = 1;
            }
            else
            { //заданный шаг
                step = Convert.ToInt16(columnTextBox2.Text);
            }

            for (int i = start; i <= end; i = i + step)
            {
                //рисуем ячейки
                cells.SetIterationNumber(i); cells.KCalculation();
                cellsDrawer.SetCells(cells); cellsDrawer.drawCells();

                //создаём исследование
                researchLoop = new Research(application, bodyDrawer.GetFacesArray());
                researchLoop.CreateStudy();

                //прикладываем силы или фиксируем грани
                foreach (object[] act in action)
                {
                    if (act[0].Equals("fix"))
                    {
                        researchLoop.BodyParts_Select((int)act[1]);
                        researchLoop.FixFace();
                    }
                    else if (act[0].Equals("force"))
                    {
                        researchLoop.BodyParts_Select((int)act[1]);
                        researchLoop.CreateLoad((double)act[2]);
                    }
                }

                researchLoop.CreateMesh(); researchLoop.MaterialSet();
                researchLoop.RunAnalysis();

                double stress = researchLoop.GetStress(), displacement = researchLoop.GetDisplacement();

                object[] buff1 = new object[2]; //[кол-во ячеек, напряжение]
                buff1[0] = Math.Pow(2,i); buff1[1] = stress;
                dataStress.Add(buff1);

                object[] buff2 = new object[2]; //[кол-во ячеек, перемещение]
                buff2[0] = Math.Pow(2, i); buff2[1] = displacement;
                dataDisplacement.Add(buff2);

                //
                //
                //
                //
                //
                //Create ResearchResult AND Add in DataBase
                ResearchResult researchResult = new ResearchResult(detail.Id, logTextBox4.Text, i, stress, displacement);
                data.AddNewResearchResultAndUpdateDetails(researchResult);
                //Create ResearchResult AND Add in DataBase
                //
                //
                //
                //
                //


                logTextBox4.Text += Environment.NewLine;
                logTextBox4.Text += "Итерация: " + i.ToString(); logTextBox4.Text += Environment.NewLine;
                logTextBox4.Text += "Напряжение(ksi): " + stress.ToString(); logTextBox4.Text += Environment.NewLine;
                logTextBox4.Text += "Смещение(mk): " + displacement.ToString(); logTextBox4.Text += Environment.NewLine;
               
                // задержка
                Thread.Sleep(2500);

                // Очищаем
                researchLoop.deleteStudy(); researchLoop = null;
                application.swModel.ClearSelection2(true);

                if (i != end) { cellsDrawer.deleteCells(); }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            startResearch4.Visible = true; deleteResearch4.Visible = false;
            diagramButton.Visible = false;
            logTextBox4.Clear();

            if (researchLoop != null) { researchLoop.deleteStudy(); researchLoop = null; }
            cellsDrawer.deleteCells();
            checkBox1.Checked = false;
        }

        private void acceptButton4_Click(object sender, EventArgs e)
        {
            researchLoop.BodyParts_Select(bodyPartsComboBox4.SelectedIndex);

            if (fixRadioButton4.Checked) {
                object[] buff = new object[3];
                buff[0] = "fix"; //действие
                buff[1] = bodyPartsComboBox4.SelectedIndex; //сторона
                buff[2] = null; //сила

                logTextBox4.Text += "Сторона " + bodyPartsComboBox4.SelectedIndex.ToString() +
                    " будет зафиксирована";
                logTextBox4.Text += Environment.NewLine;

                action.Add(buff);
            }
            else {
                object[] buff = new object[3];
                buff[0] = "force"; //действие
                buff[1] = bodyPartsComboBox4.SelectedIndex; //сторона
                buff[2] = Convert.ToDouble(textBox13.Text); //сила

                logTextBox4.Text += "На сторону " + bodyPartsComboBox4.SelectedIndex.ToString() +
                    " будет приложена " + textBox13.Text + " сила";
                logTextBox4.Text += Environment.NewLine;

                action.Add(buff);
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            action.Remove(action.Last());
            logTextBox4.Clear();

            foreach (object[] act in action)
            {
                if (act[0].Equals("fix"))
                {
                    logTextBox4.Text += "Сторона " + act[1].ToString() + " будет зафиксирована";
                    logTextBox4.Text += Environment.NewLine;
                }
                else if (act[0].Equals("force"))
                {
                    logTextBox4.Text += "На сторону " + act[1] + " будет приложена " + act[2].ToString() + " сила";
                    logTextBox4.Text += Environment.NewLine;
                }
            }
        }

        private void diagramButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            // Для обращения дочерней формы к методам материнской(передача данных)
            form2.Owner = this;
            form2.Show();
        }

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public List<object[]> GetStress() { return dataStress; }
        public List<object[]> GetDisplacement() { return dataDisplacement; }

        private void checkUseSolid_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseSolid.Checked)
            {
                drawBody.Text = "Построить тело";
                deleteBody.Text = "Удалить тело";
            }
            else
            {
                drawBody.Text = "Сохранить настройки";
                deleteBody.Text = "Сбросить настройки";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox6.Visible = true;
                researchLoop = new Research(application, bodyDrawer.GetFacesArray());

                bodyPartsComboBox4.Items.Clear();

                for (int i = 0; i < bodyDrawer.GetFacesArray().Length; i++)
                {
                    bodyPartsComboBox4.Items.Add(String.Join(", ", bodyDrawer.GetFacesArray().GetValue(i).ToString()));
                }

                action = new List<object[]>();
            }
            else
            {
                groupBox6.Visible = false;
                researchLoop = null;
                action = null;
            }

        }

        private void possibilities_Selected(object sender, EventArgs e)
        {
            if(possibilities.SelectedIndex == 1)
            {
                iterationRadioButton2.Checked = iterationRadioButton1.Checked; iterationTextBox2.Text = iterationTextBox1.Text;
                cellsNumRadioButton2.Checked = cellsNumRadioButton1.Checked; rowTextBox2.Text = rowTextBox1.Text;
                loopRadioButton2.Checked = loopRadioButton1.Checked; columnTextBox2.Text = columnTextBox1.Text;

                multiplierRadioButton2.Checked = multiplierRadioButton1.Checked; multiplierTextBox2.Text = multiplierTextBox1.Text;
                fractionRadioButton2.Checked = fractionRadioButton1.Checked; denominatorTextBox2.Text = denominatorTextBox1.Text;

                accuracyTextBox2.Text = accuracyTextBox1.Text;
            }
            else //if (possibilities.SelectedIndex == 1 || )
            {
                iterationRadioButton1.Checked = iterationRadioButton2.Checked; iterationTextBox1.Text = iterationTextBox2.Text;
                cellsNumRadioButton1.Checked = cellsNumRadioButton2.Checked; rowTextBox1.Text = rowTextBox2.Text;
                loopRadioButton1.Checked = loopRadioButton2.Checked; columnTextBox1.Text = columnTextBox2.Text;

                multiplierRadioButton2.Checked = multiplierRadioButton1.Checked; multiplierTextBox2.Text = multiplierTextBox1.Text;
                fractionRadioButton2.Checked = fractionRadioButton1.Checked; denominatorTextBox2.Text = denominatorTextBox1.Text;

                accuracyTextBox1.Text = accuracyTextBox2.Text;
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            label14.Text = "Итерация"; label13.Text = "Строка"; label12.Text = "Столбец";
            iterationTextBox2.Enabled = true;
            label13.Visible = false; rowTextBox2.Visible = false;
            label12.Visible = false; columnTextBox2.Visible = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            label14.Text = "Итерация"; label13.Text = "Строка"; label12.Text = "Столбец";
            iterationTextBox2.Enabled = false;
            label13.Visible = true; rowTextBox2.Visible = true;
            label12.Visible = true; columnTextBox2.Visible = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            label14.Text = "Начало"; label13.Text = "Конец"; label12.Text = "Шаг";
            iterationTextBox2.Enabled = true;
            label13.Visible = true; rowTextBox2.Visible = true;
            label12.Visible = true; columnTextBox2.Visible = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            label11.Text = "Множитель";
            label10.Visible = false; denominatorTextBox2.Visible = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "Числитель";
            label10.Visible = true; denominatorTextBox2.Visible = true;
        }

        private void FixRadioButton_CheckedChanged_1(object sender, EventArgs e)
        {
            acceptButton3.Text = "Зафиксировать";
            label15.Visible = false; pressureTextBox3.Visible = false;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            acceptButton3.Text = "Приложить силу";
            label15.Visible = true; pressureTextBox3.Visible = true;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            acceptButton4.Text = "Зафиксировать";
            label19.Visible = false; textBox13.Visible = false;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            acceptButton4.Text = "Приложить силу";
            label19.Visible = true; textBox13.Visible = true;
        }

        private void cubeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bodyHeightText.Visible = false; label2.Visible = false;
            bodyLenghtText.Visible = false; label3.Visible = false;
            label4.Visible = false;
        }

        private void paralRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bodyHeightText.Visible = true; label2.Visible = true;
            bodyLenghtText.Visible = true; label3.Visible = true;
            label4.Visible = true;
        }

        private void iterationRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Итерация"; label6.Text = "Строка"; label7.Text = "Столбец";
            iterationTextBox1.Enabled = true;
            label6.Visible = false; rowTextBox1.Visible = false;
            label7.Visible = false; columnTextBox1.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Итерация"; label6.Text = "Строка"; label7.Text = "Столбец";
            iterationTextBox1.Enabled = false;
            label6.Visible = true; rowTextBox1.Visible = true;
            label7.Visible = true; columnTextBox1.Visible = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label5.Text = "Начало"; label6.Text = "Конец"; label7.Text = "Шаг";
            iterationTextBox1.Enabled = true;
            label6.Visible = true; rowTextBox1.Visible = true;
            label7.Visible = true; columnTextBox1.Visible = true;
        }

        private void multiplierCheck_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "Множитель";
            label8.Visible = false; denominatorTextBox1.Visible = false;
        }

        private void FractionCheck_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "Числитель";
            label8.Visible = true; denominatorTextBox1.Visible = true;
        }

        private void bodyPartsComboBox3_SelectedIndexChanged(object sender, EventArgs e) {
            research.BodyParts_Select(bodyPartsComboBox3.SelectedIndex); }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            researchLoop.BodyParts_Select(bodyPartsComboBox4.SelectedIndex);  }


        //GOVNOKOD

        Data data;
        Detail detail;
        ISQLWorker sqlWorker;

        private void loadData()
        {
            data = new Data(sqlWorker);
            data.loadAll();
        }

        private void getDetail(string name)
        {
            detail = data.details.FindLast(x => x.Name.Equals(name));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnectionPage page = new DBConnectionPage();
                page.ShowDialog();

                sqlWorker = page.GetSQLWorker();

                loadData();

                showDetails(comboBox1);

            } catch (SQLWorkerException ex)
            {
                DialogManager.showDialogError("DataBase Error.");
            }
        }


        private void showDetails(ComboBox cmb)
        {
            cmb.Items.Clear();
            foreach (Detail d in data.details)
            {
                cmb.Items.Add(d.Name);
            }
        }

        private string getStringValue(List<Property> list, string search)
        {
            Property p = list.FindLast(x => x.Name.Equals(search));
            if (p==null || p.Value == null)
            {
                return "";
            }
            else
            {
                return p.Value.value.ToString();
            }
            
        }

        private void cmbBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0 && sqlWorker == null)
            {
                if (DialogManager.showDialogYesNo("Connect to DataBase?"))
                {
                    this.button1_Click(sender, e);
                } else
                {
                    return;
                }
            } 

            //possibilities.Show();

            getDetail(comboBox1.SelectedItem.ToString());

            if (detail != null)
            {               
                paralRadioButton.Checked = true;

                bodyWidthText.Text = getStringValue(detail.Properties, "Width");
                bodyHeightText.Text = getStringValue(detail.Properties, "Height");
                bodyLenghtText.Text = getStringValue(detail.Properties, "Lenght");

                iterationTextBox1.Text = getStringValue(detail.CellStructure.Properties, "Number Iteration");
                rowTextBox1.Text = getStringValue(detail.CellStructure.Properties, "Cell quantity");
                columnTextBox1.Text = getStringValue(detail.CellStructure.Properties, "Сyclic construction");                

            } else
            {
                DialogManager.showDialogError("Detail no found.");
            }
        }
    }
}