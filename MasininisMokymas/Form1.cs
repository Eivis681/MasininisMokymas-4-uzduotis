using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasininisMokymas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateInterface();
        }
        public void UpdateInterface()
        {
            Database database = new Database();
            List<Human> results = database.GetData();
            for (int i = 0; i< results.Count;i++)
            {
                listViewPatient.Items.Add(results[i].vardas);
                listViewPatient.Items[i].SubItems.Add(results[i].lytis.ToString());
                listViewPatient.Items[i].SubItems.Add(results[i].ugis.ToString() == "0" ? "?": results[i].ugis.ToString());
                listViewPatient.Items[i].SubItems.Add(results[i].svoris.ToString() == "0" ? "?" : results[i].svoris.ToString());

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int asd = 7 / 2;
            Database database = new Database();
            List<Human> human = database.GetData();
            Vidurkio(human);
            Medianos(human);
            Korealiacijos(human);
        }

        public void Vidurkio(List<Human> human)
        {
            double vidurkis = 0;
            int skipCounter = 0;
            for (int i = 0; i< human.Count;i++)
            {
                if (human[i].ugis==0)
                {
                    skipCounter++;
                    continue;
                }
                vidurkis = vidurkis + human[i].ugis;
            }
            vidurkis = vidurkis / (human.Count- skipCounter);
            UpdateVidurkio(vidurkis);
        }
        public void UpdateVidurkio(double vidurkis)
        {
            listBoxVidurkis.Items.Add("Paciento ūgis: " + vidurkis.ToString());
        }

        public void Medianos(List<Human> human)
        {
            var man = human.OrderBy(p => p.ugis).ToList();
            string sarasas = null;
            for(int i = 0; i< man.Count;i++)
            {
                if (man[i].ugis == 0)
                {
                    man.RemoveAt(i);
                    i--;
                    continue;
                }
                sarasas = sarasas + " " + man[i].ugis;
            }
            UpdateMedianos("Surušiuotas sąrašas:" + sarasas);
            if (man.Count %2==0)
            {
                //lyginis
                double vidurkis = 0;
                int a = man.Count;
                int b = man.Count + 1;
                vidurkis = (man[a].ugis + man[b].ugis) /2;
                UpdateMedianos("Mediana: " + vidurkis.ToString());
            }
            else
            {
                UpdateMedianos("Mediana: " + man[man.Count/2].ugis);
            }
        }
        public void UpdateMedianos(string data)
        {
            listBoxMediana.Items.Add(data);
        }

        public void Korealiacijos(List<Human> human)
        {

        }
        public void UpdateKorealiacijos(List<Human> human)
        {

        }
    }
}
