using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace disconnectedModeActivite3
{
    public partial class Form1 : Form
    {
        private DataSet MonData = new DataSet("MonData");
        public Form1()
        {
            InitializeComponent();
        }

        //Permet de créer une colonne et ajouter a MaTable
        private void addCol(
            DataTable MaTable,
            string nom,
            string Coltype,
            Boolean Index = false
            )
        {
        DataColumn dc = new DataColumn(nom, Type.GetType(Coltype));
         if(Index)
         {
                dc.Unique = true;
                dc.AutoIncrement = true;
                dc.AllowDBNull = false;
                dc.ReadOnly = true;
         }
            MaTable.Columns.Add(dc);
        }

        //ajouter des lignes à la table Passion
        private void addRow(
            DataTable MaTable,
            string[] passions
            )
        {
            DataRow row;//Reference
            foreach(string passion in passions)
            {
                row = MaTable.NewRow();
                row["Passion"] = passion;
                //Ajout

                MaTable.Rows.Add(row);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Creer et definition de la table Personnes
            DataTable table = new DataTable("Personnes");
            addCol(table, "IndexNom", "System.Int16", true);
            addCol(table, "Prenom", "System.String");
            addCol(table, "Nom", "System.String");
            table.PrimaryKey = new DataColumn[] {
               table.Columns["IndexNom"]
           };

            ArrayList Maliste = new ArrayList();
            Maliste.Add(new object[] { "Jean", "Bon"});
            Maliste.Add(new object[] { "Pierre", "Ade" });
            Maliste.Add(new object[] { "Jacques", "Adis" });
            Maliste.Add(new object[] { "Leon", "Nard" });
            Maliste.Add(new object[] { "Jean", "Tand" });
            

            foreach(object[] obj in Maliste)
            {
                DataRow row = table.NewRow();
                row["Prenom"] = obj[0];
                row["Nom"] = obj[1];

                table.Rows.Add(row);
            }

            //Ajouter le Datatable au Dataset
            MonData.Tables.Add(table);
            grdPers.DataSource = MonData;
            grdPers.DataMember = "Personnes";

            //Creation de la table Passion

            table = new DataTable("Passions");
            addCol(table, "IndexPassion", "System.Int16", true);
            addCol(table, "Passion", "System.String");
            table.PrimaryKey = new DataColumn[] {
               table.Columns["IndexPassion"]
           };

            addRow(table, 
                new string[] { "ski", "marche", "lecture", "philatelie","informatique" });

            //Ajouter le Datatable au Dataset
            MonData.Tables.Add(table);
            grdPass.DataSource = MonData;
            grdPass.DataMember = "Passions";

            //créer la table PressPass
            table = new DataTable("PersPass");
            addCol(table, "IndexNom", "System.Int16");
            addCol(table, "IndexPassion", "System.Int16");

            //Ajouter le DataTable au Dataset
            MonData.Tables.Add(table);
            grdPersPass.DataSource = MonData;
            grdPersPass.DataMember = "PersPass";

            //Definir la cle de la table
            /* table.PrimaryKey = new DataColumn[] {
                 table.Columns[0],table.Columns[1]
             };*/
            table.PrimaryKey = new DataColumn[] {
               table.Columns["IndexNom"],table.Columns["IndexPassion"]
           };

            Random r = new Random();
            short c, MaxPers, MaxPass;

            MaxPers = (short)MonData.Tables["Personnes"].Rows.Count;
            MaxPass = (short)MonData.Tables["Passions"].Rows.Count;

            for(c=0;c<=12;c++ )
            {
                DataRow row = table.NewRow();

                row.ItemArray = 
                    new object[] { r.Next(MaxPers),r.Next(MaxPass)};

                try
                {
                    table.Rows.Add(row);
                }
                catch(Exception)
                {
                    //Violation de cles primaire
                }
            }

            //Declaration des relations 

            DataRelation rel =
                new DataRelation(
                    "PersInt",
                    MonData.Tables["Personnes"].Columns["IndexNom"],
                    MonData.Tables["PersPass"].Columns["IndexNom"]
                );

            MonData.Relations.Add(rel);

            rel =
                new DataRelation(
                    "PassInt",
                    MonData.Tables["Passions"].Columns["IndexPassion"],
                    MonData.Tables["PersPass"].Columns["IndexPassion"]
                );

            MonData.Relations.Add(rel);

        }
    }
}