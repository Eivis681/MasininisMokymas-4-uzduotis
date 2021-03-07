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
            listBoxKorealiacija.Items.Clear();
            listBoxMediana.Items.Clear();
            listBoxVidurkis.Items.Clear();
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
                //remove id needed
                //if (human[i].ugis==0)
                //{
                //    skipCounter++;
                //    continue;
                //}
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
                //remove if not needed 
                //if (man[i].ugis == 0)
                //{
                //    man.RemoveAt(i);
                //    i--;
                //    continue;
                //}
                sarasas = sarasas + " " + man[i].ugis;
            }
            UpdateMedianos("Surušiuotas sąrašas:" + sarasas);
            if (man.Count %2==0)
            {
                //lyginis
                double vidurkis = 0;
                int a = man.Count/2 -1;
                //int b = (man.Count + 1)/2;
                vidurkis = (man[a].ugis + man[a+1].ugis) /2;
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
            List<double> jezusMana = new List<double>();
            for (int i = 0; i< human.Count;i++)
            {
                if (human[i].ugis== 0)
                {
                    jezusMana.Add(human[i].svoris);
                    jezusMana.Add(human[i].lytis);
                    human.RemoveAt(i);
                    i--;
                }
            }
            double vidurkisUgis = 0;
            double vidurkitLytis = 0;
            double vidurkisSvoris = 0;
            for (int i = 0; i < human.Count; i++)
            {
                vidurkisUgis = vidurkisUgis + human[i].ugis;
                vidurkitLytis = vidurkitLytis + human[i].lytis;
                vidurkisSvoris = vidurkisSvoris + human[i].svoris;
            }
            vidurkisUgis = vidurkisUgis / human.Count;
            vidurkisSvoris = vidurkisSvoris / human.Count;
            vidurkitLytis = vidurkitLytis / human.Count;
            UpdateKorealiacijos("Ūgio vidurkis: " + vidurkisUgis);
            UpdateKorealiacijos("Svorio vidurkis: " + vidurkisSvoris);
            UpdateKorealiacijos("Lyties vidurkis: " + vidurkitLytis);
            List<double> svoris = new List<double>();
            List<double> ugis = new List<double>();
            List<double> lytis = new List<double>();
            for (int i = 0; i< human.Count;i++)
            {
                svoris.Add(human[i].svoris);
                ugis.Add(human[i].ugis);
                lytis.Add(human[i].lytis);
            }

            var avgUgis = ugis.Average();
            var avgSvoris = svoris.Average();
            var avgLytis = lytis.Average();

            var sum1 = ugis.Zip(svoris, (x1, y1) => (x1 - avgUgis) * (y1 - avgSvoris)).Sum();
            var sum2 = ugis.Zip(lytis, (x1, y1) => (x1 - avgUgis) * (y1 - avgLytis)).Sum();

            var sumSqr1 = ugis.Sum(x => Math.Pow((x - avgUgis), 2.0));
            var sumSqr2 = svoris.Sum(y => Math.Pow((y - avgSvoris), 2.0));
            var sumSqr3 = lytis.Sum(y => Math.Pow((y - avgLytis), 2.0));

            var resultUgisSvoris = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);
            var resultUgisLytis = sum2 / Math.Sqrt(sumSqr1 * sumSqr3);

            UpdateKorealiacijos("Ugis svoris korealiacija: " + resultUgisSvoris);
            UpdateKorealiacijos("Ugis lytis korealiacija: " + resultUgisLytis);

            double skaicius = 1 / (resultUgisLytis+ resultUgisSvoris);
            double finalResult = vidurkisUgis + skaicius * ((resultUgisSvoris*(jezusMana[0]- vidurkisSvoris)) + (resultUgisLytis*(jezusMana[1]-vidurkitLytis)));
            UpdateKorealiacijos("Rezultatas " + finalResult);
        }
        public void UpdateKorealiacijos(string data)
        {
            listBoxKorealiacija.Items.Add(data);
        }
    }
}
