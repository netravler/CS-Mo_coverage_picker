using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Management;
using System.Device;

namespace MO_coverage_picker
{
    public partial class form1 : Form
    {
        string currentcounty = ""; // place holder for data updates
        string currentxmlfile = ""; // to pass to update reference
        string currentxsdfile = ""; // to pass to update reference
        string commonpasseddata = "";


        public form1()
        {
            InitializeComponent();
            // hide botton and datagrid
            this.btnClose.Visible = false;
            this.btnCloseCounties.Visible = false;
            this.dataGridView1.Visible = false;

            //turn the buttons off...
            turnAllButtonOff("");
            turnOnButton(getVisableCounties(@"c:\coverage\States\Missouri\coveragecounties.xml"));

            // hb serial please
           // if ("100731PCKB04VNH3598J" == GetHDDSerial())
           // {
                tbSerial.Text = GetHDDSerial();
           // }
           // else
           // {
           //     tbSerial.Text = "Unregisted Computer";
           //     turnAllButtonOff("");
           //     btnCounty.Visible = false;
           // }

         }

        public void btnClose_Click(object sender, EventArgs e)
        {
            // hide bottonand datagrid
            this.btnClose.Visible = false;
            this.dataGridView1.Visible = false;
            this.dataGridView1.ReadOnly = false;

            DataSet ds = (DataSet)dataGridView1.DataSource;
            ds.WriteXml(commonpasseddata, XmlWriteMode.IgnoreSchema);

            // repaint updated working counties
            turnAllButtonOff("");
            turnOnButton(getVisableCounties(@"c:\coverage\States\Missouri\coveragecounties.xml"));
            //
        }

        private void btnCloseCounties_Click(object sender, EventArgs e)
        {
            // hide bottonand datagrid
            this.btnCloseCounties.Visible = false;
            this.dataGridView1.Visible = false;
            this.dataGridView1.ReadOnly = false;

            // repaint updated working counties
            turnAllButtonOff("");
            turnOnButton(getVisableCounties(@"c:\coverage\States\Missouri\coveragecounties.xml"));
            //
        }

        private void btnCounty_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\coveragecounties.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\coveragecounties.xsd";

            populateDatagrid("coveragecounties", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.dataGridView1.Columns["state"].ReadOnly = true;
            this.dataGridView1.Columns["county"].ReadOnly = true;

            this.btnClose.Visible = true;
            this.dataGridView1.Visible = true;

            commonpasseddata = currentxmlfile;
         }

        private void populateDatagrid(string dataIn, string myxsd, string myxml, string stuff)
        {
            // turn all buttons off for the duration of the edit
            turnAllButtonOff("");
            //
            // datagrid load
            // stuff here
            //DataSet ds = new DataSet("Info");
            //DataTable dt = new DataTable("Record");
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string xsd = myxsd;
            string xml = myxml;

            if (!File.Exists(xsd))
            {
                dt.Columns.Add("Sequence");
                dt.Columns.Add("TransID");
                ds.Tables.Add(dt);
            }
            else
            {
                ds.ReadXmlSchema(xsd);
                ds.ReadXml(xml);
            }
            
            //Binds datagrid this is the common datagrid for data interactions
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = dataIn;
            //commonpasseddata = dataIn;

        }

        static string GetXmlString(string strFile)
        {
            // Load the xml file into XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Now create StringWriter object to get data from xml document.
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xw);
            return sw.ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataSet ds = (DataSet)dataGridView1.DataSource;
            ds.WriteXml(commonpasseddata, XmlWriteMode.IgnoreSchema);
        }

        private string getVisableCounties(string countyxmlfile)
        {
            // We construct a long string of counties to show.  
            string fullList = GetXmlString(countyxmlfile);
            string County = "";
            string YORN = "";
            string passback = "";

            using (StringReader stringReader = new StringReader(fullList)) // so
            using (XmlTextReader reader = new XmlTextReader(stringReader))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            // fill elements to snatch in the case of county we only want the county that has a Y...
                            case "state":
                                break;

                            case "county":
                                // hold for y/n check
                                County = reader.ReadString();
                                break;
                                
                            case "showyn":
                                // Y or N not using N's
                                YORN = reader.ReadString();
                                if (YORN == "Y" || YORN == "y")
                                {
                                    passback += County;
                                }
                                break;
                        }
                    }
                }
            
            }

            return passback;
        }

        private void turnOnButton(string countyListing)
        {
            // We are passed a string containing all the counties to show...
            if  (countyListing.Contains("Jackson"))
            {
 	        this.btnJackson.Visible = true;
            }
            if  (countyListing.Contains("Clay"))
            {
 	        this.btnClay.Visible = true;
            }

            if (countyListing.Contains("Atchison"))
            {
                this.btnRay.Visible = true;
            }

            if (countyListing.Contains("Adain"))
            {
                this.btnClinton.Visible = true;
            }

            if  (countyListing.Contains("Ray"))
            {
 	        this.btnRay.Visible = true;
            }
  
            if  (countyListing.Contains("Clinton"))
            {
 	        this.btnClinton.Visible = true;
            }
  
            if  (countyListing.Contains("Cass"))
            {
 	        this.btnCass.Visible = true;
            }
  
            if  (countyListing.Contains("Bates"))
            {
 	        this.btnBates.Visible = true;
            }
  
            if  (countyListing.Contains("Platte"))
            {
 	        this.btnPlatte.Visible = true;
            }
  
            if  (countyListing.Contains("Buchanan"))
            {
 	        this.btnBuchanan.Visible = true;
            }
  
            if  (countyListing.Contains("Atchison"))
            {
 	        this.btnAtchison.Visible = true;
            }
  
            if  (countyListing.Contains("Holt"))
            {
 	        this.btnHolt.Visible = true;
            }
  
            if  (countyListing.Contains("Nodaway"))
            {
 	        this.btnNodaway.Visible = true;
            }
  
            if  (countyListing.Contains("Andrew"))
            {
 	        this.btnAndrew.Visible = true;
            }
  
            if  (countyListing.Contains("Vernon"))
            {
 	        this.btnVernon.Visible = true;
            }
  
            if  (countyListing.Contains("Barton"))
            {
 	        this.btnBarton.Visible = true;
            }
  
            if  (countyListing.Contains("Jasper"))
            {
 	        this.btnJasper.Visible = true;
            }
  
            if  (countyListing.Contains("Newton"))
            {
 	        this.btnNewton.Visible = true;
            }
  
            if  (countyListing.Contains("McDonald"))
            {
 	        this.btnMcDonald.Visible = true;
            }
  
            if  (countyListing.Contains("Worth"))
            {
 	        this.btnWorth.Visible = true;
            }
  
            if  (countyListing.Contains("Gentry"))
            {
 	        this.btnGentry.Visible = true;
            }
  
            if  (countyListing.Contains("Daviess"))
            {
 	        this.btnDaviess.Visible = true;
            }
  
            if  (countyListing.Contains("Harrison"))
            {
 	        this.btnHarrison.Visible = true;
            }
  
            if  (countyListing.Contains("Mercer"))
            {
 	        this.btnMercer.Visible = true;
            }
  
            if  (countyListing.Contains("Putnam"))
            {
 	        this.btnPutnam.Visible = true;
            }
  
            if  (countyListing.Contains("Johnson"))
            {
 	        this.btnJohnson.Visible = true;
            }
  
            if  (countyListing.Contains("Henry"))
            {
 	        this.btnHenry.Visible = true;
            }
  
            if  (countyListing.Contains("StClair"))
            {
 	        this.btnStClair.Visible = true;
            }
  
            if  (countyListing.Contains("Ceder"))
            {
 	        this.btnCeder.Visible = true;
            }
  
            if  (countyListing.Contains("Dade"))
            {
 	        this.btnDade.Visible = true;
            }
  
            if  (countyListing.Contains("Lawrence"))
            {
 	        this.btnLawrence.Visible = true;
            }
  
            if  (countyListing.Contains("Barry"))
            {
 	        this.btnBarry.Visible = true;
            }
  
            if  (countyListing.Contains("Livingston"))
            {
 	        this.btnLivingston.Visible = true;
            }
  
            if  (countyListing.Contains("Grundy"))
            {
 	        this.btnGrundy.Visible = true;
            }
  
            if  (countyListing.Contains("Carroll"))
            {
 	        this.btnCarroll.Visible = true;
            }
  
            if  (countyListing.Contains("Lafayette"))
            {
 	        this.btnLafayette.Visible = true;
            }
  
            if  (countyListing.Contains("Saline"))
            {
 	        this.btnSaline.Visible = true;
            }
  
            if  (countyListing.Contains("Pettis"))
            {
 	        this.btnPettis.Visible = true;
            }
  
            if  (countyListing.Contains("Benton"))
            {
 	        this.btnBenton.Visible = true;
            }
  
            if  (countyListing.Contains("Hickory"))
            {
 	        this.btnHickory.Visible = true;
            }
  
            if  (countyListing.Contains("Polk"))
            {
 	        this.btnPolk.Visible = true;
            }
  
            if  (countyListing.Contains("Greene"))
            {
 	        this.btnGreene.Visible = true;
            }
  
            if  (countyListing.Contains("Stone"))
            {
 	        this.btnStone.Visible = true;
            }
  
            if  (countyListing.Contains("Christian"))
            {
 	        this.btnChristian.Visible = true;
            }
  
            if  (countyListing.Contains("Taney"))
            {
 	        this.btnTaney.Visible = true;
            }
  
            if  (countyListing.Contains("Webster"))
            {
 	        this.btnWebster.Visible = true;
            }
  
            if  (countyListing.Contains("Wright"))
            {
 	        this.btnWright.Visible = true;
            }
  
            if  (countyListing.Contains("Texas"))
            {
 	        this.btnTexas.Visible = true;
            }
  
            if  (countyListing.Contains("Ozark"))
            {
 	        this.btnOzark.Visible = true;
            }
  
            if  (countyListing.Contains("Howell"))
            {
 	        this.btnHowell.Visible = true;
            }
  
            if  (countyListing.Contains("Shannon"))
            {
 	        this.btnShannon.Visible = true;
            }
  
            if  (countyListing.Contains("Oregon"))
            {
 	        this.btnOregon.Visible = true;
            }
  
            if  (countyListing.Contains("Reynolds"))
            {
 	        this.btnReynolds.Visible = true;
            }
  
            if  (countyListing.Contains("Wayne"))
            {
 	        this.btnWayne.Visible = true;
            }
  
            if  (countyListing.Contains("Madison"))
            {
 	        this.btnMadison.Visible = true;
            }
  
            if  (countyListing.Contains("Butler"))
            {
 	        this.btnButler.Visible = true;
            }
  
            if  (countyListing.Contains("Stoddard"))
            {
 	        this.btnStoddard.Visible = true;
            }
  
            if  (countyListing.Contains("Carter"))
            {
 	        this.btnCarter.Visible = true;
            }
  
            if  (countyListing.Contains("Ripley"))
            {
 	        this.btnRipley.Visible = true;
            }
  
            if  (countyListing.Contains("NewMadrid"))
            {
 	        this.btnNewMadrid.Visible = true;
            }
  
            if  (countyListing.Contains("Mississippi"))
            {
 	        this.btnMississippi.Visible = true;
            }
  
            if  (countyListing.Contains("Dunkin"))
            {
 	        this.btnDunkin.Visible = true;
            }
  
            if  (countyListing.Contains("Panopscott"))
            {
 	        this.btnPanopscott.Visible = true;
            }
  
            if  (countyListing.Contains("Scott"))
            {
 	        this.btnScott.Visible = true;
            }
  
            if  (countyListing.Contains("Laclede"))
            {
 	        this.btnLaclede.Visible = true;
            }
  
            if  (countyListing.Contains("Dallas"))
            {
 	        this.btnDallas.Visible = true;
            }
  
            if  (countyListing.Contains("Dent"))
            {
 	        this.btnDent.Visible = true;
            }
  
            if  (countyListing.Contains("Callaway"))
            {
 	        this.btnCallaway.Visible = true;
            }
  
            if  (countyListing.Contains("Pulaski"))
            {
 	        this.btnPulaski.Visible = true;
            }
  
            if  (countyListing.Contains("Franklin"))
            {
 	        this.btnFranklin.Visible = true;
            }
  
            if  (countyListing.Contains("Miller"))
            {
 	        this.btnMiller.Visible = true;
            }
  
            if  (countyListing.Contains("Pike"))
            {
 	        this.btnPike.Visible = true;
            }
  
            if  (countyListing.Contains("Lincoln"))
            {
 	        this.btnLincoln.Visible = true;
            }
  
            if  (countyListing.Contains("Sullivan"))
            {
 	        this.btnSullivan.Visible = true;
            }
  
            if  (countyListing.Contains("Macon"))
            {
 	        this.btnMacon.Visible = true;
            }
  
            if  (countyListing.Contains("Chariton"))
            {
 	        this.btnChariton.Visible = true;
            }
  
            if  (countyListing.Contains("Morgan"))
            {
 	        this.btnMorgan.Visible = true;
            }
  
            if  (countyListing.Contains("Linn"))
            {
 	        this.btnLinn.Visible = true;
            }
  
            if  (countyListing.Contains("Adair"))
            {
 	        this.btnAdair.Visible = true;
            }
  
            if  (countyListing.Contains("Monroe"))
            {
 	        this.btnMonroe.Visible = true;
            }
  
            if  (countyListing.Contains("Clark"))
            {
 	        this.btnClark.Visible = true;
            }
  
            if  (countyListing.Contains("Lewis"))
            {
 	        this.btnLewis.Visible = true;
            }
  
            if  (countyListing.Contains("Cooper"))
            {
 	        this.btnCooper.Visible = true;
            }
  
            if  (countyListing.Contains("Boone"))
            {
 	        this.btnBoone.Visible = true;
            }
  
            if  (countyListing.Contains("Maries"))
            {
 	        this.btnMaries.Visible = true;
            }
  
            if  (countyListing.Contains("Audrain"))
            {
 	        this.btnAudrain.Visible = true;
            }
  
            if  (countyListing.Contains("Ralls"))
            {
 	        this.btnRalls.Visible = true;
            }
  
            if  (countyListing.Contains("Crawford"))
            {
 	        this.btnCrawford.Visible = true;
            }
  
            if  (countyListing.Contains("Phelps"))
            {
 	        this.btnPhelps.Visible = true;
            }
  
            if  (countyListing.Contains("Douglas"))
            {
 	        this.btnDouglas.Visible = true;
            }
  
            if  (countyListing.Contains("Washington"))
            {
 	        this.btnWashington.Visible = true;
            }
  
            if  (countyListing.Contains("StFrancis"))
            {
 	        this.btnStFrancis.Visible = true;
            }
  
            if  (countyListing.Contains("Perry"))
            {
 	        this.btnPerry.Visible = true;
            }
  
            if  (countyListing.Contains("StLouis"))
            {
 	        this.btnStLouis.Visible = true;
            }
  
            if  (countyListing.Contains("Scotland"))
            {
 	        this.btnScotland.Visible = true;
            }
  
            if  (countyListing.Contains("Schuyler"))
            {
 	        this.btnSchuyler.Visible = true;
            }
  
            if  (countyListing.Contains("Knox"))
            {
 	        this.btnKnox.Visible = true;
            }
  
            if  (countyListing.Contains("Randolph"))
            {
 	        this.btnRandolph.Visible = true;
            }
  
            if  (countyListing.Contains("Moniteau"))
            {
 	        this.btnMoniteau.Visible = true;
            }
  
            if  (countyListing.Contains("StCharles"))
            {
 	        this.btnStCharles.Visible = true;
            }
  
            if  (countyListing.Contains("Gasconade"))
            {
 	        this.btnGasconade.Visible = true;
            }
  
            if  (countyListing.Contains("Camden"))
            {
 	        this.btnCamden.Visible = true;
            }
  
            if  (countyListing.Contains("Warren"))
            {
 	        this.btnWarren.Visible = true;
            }
  
            if  (countyListing.Contains("Montgomery"))
            {
 	        this.btnMontgomery.Visible = true;
            }
  
            if  (countyListing.Contains("Osage"))
            {
 	        this.btnOsage.Visible = true;
            }
  
            if  (countyListing.Contains("Jefferson"))
            {
 	        this.btnJefferson.Visible = true;
            }
  
            if  (countyListing.Contains("CapeGirardeau"))
            {
 	        this.btnCapeGirardeau.Visible = true;
            }
        }

        private void turnAllButtonOff(string countyListing)
        {
            // A string is passed to this function but currently isn't used...
            this.btnJackson.Visible = false;
            this.btnClay.Visible = false;
            this.btnRay.Visible = false;
            this.btnAtchison.Visible = false;
            this.btnAdair.Visible = false;
            this.btnClinton.Visible = false;
            this.btnCass.Visible = false;
            this.btnBates.Visible = false;
            this.btnPlatte.Visible = false;
            this.btnBuchanan.Visible = false;
            this.btnAtchison.Visible = false;
            this.btnHolt.Visible = false;
            this.btnNodaway.Visible = false;
            this.btnAndrew.Visible = false;
            this.btnVernon.Visible = false;
            this.btnBarton.Visible = false;
            this.btnJasper.Visible = false;
            this.btnNewton.Visible = false;
            this.btnMcDonald.Visible = false;
            this.btnWorth.Visible = false;
            this.btnGentry.Visible = false;
            this.btnDaviess.Visible = false;
            this.btnHarrison.Visible = false;
            this.btnMercer.Visible = false;
            this.btnPutnam.Visible = false;
            this.btnJohnson.Visible = false;
            this.btnHenry.Visible = false;
            this.btnStClair.Visible = false;
            this.btnCeder.Visible = false;
            this.btnDade.Visible = false;
            this.btnLawrence.Visible = false;
            this.btnBarry.Visible = false;
            this.btnLivingston.Visible = false;
            this.btnGrundy.Visible = false;
            this.btnCarroll.Visible = false;
            this.btnLafayette.Visible = false;
            this.btnSaline.Visible = false;
            this.btnPettis.Visible = false;
            this.btnBenton.Visible = false;
            this.btnHickory.Visible = false;
            this.btnPolk.Visible = false;
            this.btnGreene.Visible = false;
            this.btnStone.Visible = false;
            this.btnChristian.Visible = false;
            this.btnTaney.Visible = false;
            this.btnWebster.Visible = false;
            this.btnWright.Visible = false;
            this.btnTexas.Visible = false;
            this.btnOzark.Visible = false;
            this.btnHowell.Visible = false;
            this.btnShannon.Visible = false;
            this.btnOregon.Visible = false;
            this.btnReynolds.Visible = false;
            this.btnWayne.Visible = false;
            this.btnMadison.Visible = false;
            this.btnButler.Visible = false;
            this.btnStoddard.Visible = false;
            this.btnCarter.Visible = false;
            this.btnRipley.Visible = false;
            this.btnNewMadrid.Visible = false;
            this.button1.Visible = false;
            this.btnMississippi.Visible = false;
            this.btnDunkin.Visible = false;
            this.btnPanopscott.Visible = false;
            this.btnScott.Visible = false;
            this.btnLaclede.Visible = false;
            this.btnDallas.Visible = false;
            this.btnDent.Visible = false;
            this.btnCallaway.Visible = false;
            this.btnPulaski.Visible = false;
            this.btnFranklin.Visible = false;
            this.btnMiller.Visible = false;
            this.btnPike.Visible = false;
            this.btnLincoln.Visible = false;
            this.btnSullivan.Visible = false;
            this.btnMacon.Visible = false;
            this.btnChariton.Visible = false;
            this.btnMorgan.Visible = false;
            this.btnLinn.Visible = false;
            this.btnAdair.Visible = false;
            this.btnMonroe.Visible = false;
            this.btnClark.Visible = false;
            this.btnLewis.Visible = false;
            this.btnCooper.Visible = false;
            this.btnBoone.Visible = false;
            this.btnMaries.Visible = false;
            this.btnAudrain.Visible = false;
            this.btnRalls.Visible = false;
            this.btnCrawford.Visible = false;
            this.btnPhelps.Visible = false;
            this.btnDouglas.Visible = false;
            this.btnWashington.Visible = false;
            this.btnStFrancis.Visible = false;
            this.btnPerry.Visible = false;
            this.btnStLouis.Visible = false;
            this.btnScotland.Visible = false;
            this.btnSchuyler.Visible = false;
            this.btnKnox.Visible = false;
            this.btnRandolph.Visible = false;
            this.btnMoniteau.Visible = false;
            this.btnStCharles.Visible = false;
            this.btnGasconade.Visible = false;
            this.btnCamden.Visible = false;
            this.btnWarren.Visible = false;
            this.btnMontgomery.Visible = false;
            this.btnOsage.Visible = false;
            this.btnJefferson.Visible = false;
            this.btnCapeGirardeau.Visible = false;
        }
        // what follows is a list of the buttons for editing of the counties xml files.
        // this list was created by doing a word mail merge...

         private void btnAdair_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Adair\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Adair\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnAndrew_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Andrew\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Andrew\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnAtchison_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Atchison\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Atchison\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnAudrain_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Audrain\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Audrain\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBarry_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Barry\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Barry\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBarton_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Barton\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Barton\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBates_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Bates\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Bates\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBenton_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Benton\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Benton\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBollinger_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Bollinger\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Bollinger\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBoone_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Boone\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Boone\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnBuchanan_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Buchanan\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Buchanan\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnButler_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Butler\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Butler\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCaldwell_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Caldwell\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Caldwell\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCallaway_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Callaway\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Callaway\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCamden_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Camden\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Camden\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCapeGirardeau_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Cape Girardeau\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Cape Girardeau\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCarroll_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Carroll\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Carroll\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCarter_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Carter\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Carter\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCass_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Cass\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Cass\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCedar_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Cedar\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Cedar\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnChariton_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Chariton\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Chariton\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnChristian_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Christian\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Christian\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnClark_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Clark\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Clark\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnClay_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Clay\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Clay\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnClinton_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Clinton\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Clinton\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCole_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Cole\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Cole\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCooper_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Cooper\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Cooper\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnCrawford_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Crawford\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Crawford\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDade_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Dade\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Dade\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDallas_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Dallas\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Dallas\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDaviess_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Daviess\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Daviess\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDeKalb_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\DeKalb\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\DeKalb\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDent_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Dent\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Dent\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDouglas_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Douglas\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Douglas\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnDunklin_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Dunklin\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Dunklin\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnFranklin_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Franklin\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Franklin\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnGasconade_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Gasconade\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Gasconade\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnGentry_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Gentry\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Gentry\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnGreene_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Greene\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Greene\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnGrundy_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Grundy\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Grundy\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHarrison_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Harrison\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Harrison\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHenry_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Henry\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Henry\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHickory_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Hickory\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Hickory\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHolt_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Holt\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Holt\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHoward_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Howard\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Howard\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnHowell_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Howell\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Howell\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnIron_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Iron\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Iron\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnJackson_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Jackson\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Jackson\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnJasper_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Jasper\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Jasper\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnJefferson_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Jefferson\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Jefferson\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnJohnson_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Johnson\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Johnson\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnKnox_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Knox\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Knox\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLaclede_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Laclede\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Laclede\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLafayette_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Lafayette\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Lafayette\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLawrence_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Lawrence\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Lawrence\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLewis_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Lewis\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Lewis\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLincoln_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Lincoln\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Lincoln\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLinn_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Linn\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Linn\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnLivingston_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Livingston\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Livingston\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMcDonald_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\McDonald\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\McDonald\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMacon_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Macon\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Macon\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMadison_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Madison\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Madison\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMaries_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Maries\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Maries\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMarion_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Marion\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Marion\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMercer_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Mercer\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Mercer\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMiller_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Miller\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Miller\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMississippi_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Mississippi\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Mississippi\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMoniteau_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Moniteau\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Moniteau\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMonroe_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Monroe\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Monroe\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMontgomery_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Montgomery\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Montgomery\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnMorgan_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Morgan\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Morgan\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnNewMadrid_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\New Madrid\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\New Madrid\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnNewton_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Newton\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Newton\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnNodaway_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Nodaway\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Nodaway\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnOregon_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Oregon\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Oregon\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnOsage_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Osage\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Osage\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnOzark_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ozark\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ozark\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPerry_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Perry\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Perry\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPettis_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Pettis\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Pettis\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPhelps_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Phelps\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Phelps\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPike_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Pike\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Pike\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPlatte_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Platte\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Platte\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPolk_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Polk\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Polk\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPulaski_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Pulaski\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Pulaski\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnPutnam_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Putnam\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Putnam\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnRalls_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ralls\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ralls\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnRandolph_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Randolph\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Randolph\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnRay_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ray\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ray\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnReynolds_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Reynolds\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Reynolds\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnRipley_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ripley\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ripley\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStCharles_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\St. Charles\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\St. Charles\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStClair_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\St. Clair\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\St. Clair\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnSteGenevieve_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ste. Genevieve\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ste. Genevieve\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStFrancois_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\St. Francois\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\St. Francois\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStLouis_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\St. Louis\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\St. Louis\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnSchuyler_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Schuyler\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Schuyler\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }

        private void btnDunkin_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Dunkin\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Dunkin\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;

            commonpasseddata = currentxmlfile;
        }

        private void btnAdian_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Adair\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Adair\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;

        }

        private void btnCeder_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Ceder\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Ceder\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;

        }

        private void btnPanopscott_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Panopscott\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Panopscott\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;


        }

        private void btnStFrancis_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\StFrancis\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\StFrancis\transaction.xsd";


            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;

        }


 
        private void btnScotland_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Scotland\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Scotland\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnScott_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Scott\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Scott\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnShannon_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Shannon\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Shannon\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnShelby_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Shelby\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Shelby\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStoddard_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Stoddard\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Stoddard\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnStone_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Stone\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Stone\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnSullivan_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Sullivan\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Sullivan\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnTaney_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Taney\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Taney\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnTexas_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Texas\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Texas\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnVernon_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Vernon\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Vernon\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWarren_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Warren\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Warren\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWashington_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Washington\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Washington\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWayne_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Wayne\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Wayne\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWebster_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Webster\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Webster\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWorth_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Worth\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Worth\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }
 
        private void btnWright_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Wright\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Wright\transaction.xsd";

            commonpasseddata = currentxmlfile;

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;
        }

        private void btnSaline_Click(object sender, EventArgs e)
        {
            // populateDatagrid 
            currentxmlfile = @"c:\coverage\States\Missouri\Saline\transaction.xml";
            currentxsdfile = @"c:\coverage\States\Missouri\Saline\transaction.xsd";

            populateDatagrid("TransactionHistory", currentxsdfile, currentxmlfile, "");

            // unhide botton and datagrid
            this.btnClose.Visible = true;

            this.dataGridView1.Visible = true;

            commonpasseddata = currentxmlfile;
        }

        // edit generated button edit routines

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)dataGridView1.DataSource;
            ds.WriteXml(commonpasseddata, XmlWriteMode.IgnoreSchema);
        }

        private void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {

        }

        // get the hd numbers

        public string GetHDDSerial() 
        {     
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            foreach (ManagementObject wmi_HD in searcher.Get())     
            {         // get the hardware serial no. 
                if (wmi_HD["SerialNumber"] != null) 
                    return wmi_HD["SerialNumber"].ToString();     
            }      
            return string.Empty; 
        } 
    }
}
