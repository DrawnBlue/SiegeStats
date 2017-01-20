using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Siege_Stats
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (cboRegion.SelectedIndex == -1)
            {
                cboRegion.SelectedIndex = 0;
            }

            //Reset List
            lstAliases.Items.Clear();

            //reset text boxes
            txtAccuracy.Clear();
            //txtCasualDeaths.Clear();
            txtCasualKD.Clear();
            txtCasualKills.Clear();
            txtCasualPT.Clear();
            txtCasualWL.Clear();
            txtHeadshot.Clear();
            txtLevel.Clear();
            txtMelee.Clear();
            txtMMR.Clear();
            txtOp1.Clear();
            txtOp2.Clear();
            txtOp3.Clear();
            txtOp4.Clear();
            txtOp5.Clear();
            txtOp6.Clear();
            txtRank.Clear();
            //txtRankedDeaths.Clear();
            txtRankedKD.Clear();
            txtRankedKills.Clear();
            txtRankedPT.Clear();
            txtRankedWL.Clear();
            txtUplayID.Clear();

            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                try //everything
                {
                    bool blStatsFail = false;

                    //Get stats urls
                    string casualUrl = "https://r6stats.com/stats/uplay/" + txtUsername.Text + "/casual";
                    string rankedUrl = "https://r6stats.com/stats/uplay/" + txtUsername.Text + "/ranked";

                    //Get r6db
                    string r6dbUrl = "https://r6db.com/api/v2/players?name=" + txtUsername.Text + "&exact=0";
                    var r6dbRequest = (HttpWebRequest)WebRequest.Create(r6dbUrl);
                    r6dbRequest.ContentType = "application/x-www-form-urlencoded";

                    r6dbRequest.Headers.Add("x-app-id","r6db-frontend");

                    WebResponse r6dbResponse = r6dbRequest.GetResponse();

                    Stream r6dbStream = r6dbResponse.GetResponseStream();

                    StreamReader r6dbreader = new StreamReader(r6dbStream);

                    String r6dbString = r6dbreader.ReadToEnd();

                    r6dbResponse.Close();


                    //Load Casual
                    HtmlWeb casualWeb = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument casualDoc = casualWeb.Load(casualUrl);

                    //Load Ranked
                    HtmlWeb rankedWeb = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument rankedDoc = rankedWeb.Load(rankedUrl);



                    //Show results

                    //R6stats
                    try
                    {
                        //Level
                        txtLevel.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[4]/div/div[2]")[0].InnerText;

                        //Casual
                        txtCasualKD.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[3]/div/div[2]")[0].InnerText;
                        txtCasualKills.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[1]/div/div[2]")[0].InnerText + ":" + casualDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[2]/div/div[2]")[0].InnerText;
                        //txtCasualDeaths.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[2]/div/div[2]")[0].InnerText;
                        txtCasualWL.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[2]/div/div[2]")[0].InnerText;
                        txtCasualPT.Text = casualDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[4]/div/div[2]")[0].InnerText;

                        //Ranked
                        txtRankedKD.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[3]/div/div[2]")[0].InnerText;
                        txtRankedKills.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[1]/div/div[2]")[0].InnerText + ":" + rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[2]/div/div[2]")[0].InnerText;
                        //txtRankedDeaths.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[2]/div/div[2]")[0].InnerText;
                        txtRankedWL.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[2]/div/div[2]")[0].InnerText;
                        txtRankedPT.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[4]/div/div[2]")[0].InnerText;

                        //Operators
                        txtOp1.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[1]/div/div/h3/a")[0].InnerText;
                        txtOp2.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[2]/div/div/h3/a")[0].InnerText;
                        txtOp3.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[3]/div/div/h3/a")[0].InnerText;
                        txtOp4.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[4]/div/div/h3/a")[0].InnerText;
                        txtOp5.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[5]/div/div/h3/a")[0].InnerText;
                        txtOp6.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[5]/div[1]/div[2]/div[6]/div/div/h3/a")[0].InnerText;

                        //Overall Stats
                        txtAccuracy.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[2]/td[2]")[0].InnerText;
                        txtHeadshot.Text = Convert.ToString(Decimal.Parse(rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[3]/td[2]")[0].InnerText.Replace("%", "")) * 10m) + "%";
                        txtMelee.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[1]/td[2]")[0].InnerText;
                    }
                    catch //r6stats catch
                    {
                        MessageBox.Show("Unable to get stats. Did you input the correct username?", "Error r6stats.com");
                        blStatsFail = true;
                    }

                    //R6DB try
                    try
                    {
                        //R6DB stuff
                        txtUplayID.Text = getBetween(r6dbString, "\"id\":\"", "\",");
                        decimal decRawMMR = 0;
                        if (cboRegion.SelectedIndex == 0)
                        {
                            decRawMMR = Convert.ToDecimal(getBetween(r6dbString, "\"ncsa\":{\"mmr\":", ",\""));
                        }
                        if (cboRegion.SelectedIndex == 1)
                        {
                            decRawMMR = Convert.ToDecimal(getBetween(r6dbString, "\"emea\":{\"mmr\":", ",\""));
                        }
                        if (cboRegion.SelectedIndex == 2)
                        {
                            decRawMMR = Convert.ToDecimal(getBetween(r6dbString, "\"apac\":{\"mmr\":", ",\""));
                        }
                        txtMMR.Text = Convert.ToString(Math.Round(Convert.ToDecimal(decRawMMR)));

                        //Aliases
                        showNames(r6dbString);

                        //Rank

                        if (cboRegion.SelectedIndex == 0)
                        {
                            txtRank.Text = getBetween(r6dbString, "\"ncsa\":{\"mmr\":" + decRawMMR + ",\"rank\":", "}},\"updated_at\"");
                        }

                        if (cboRegion.SelectedIndex == 1)
                        {
                            txtRank.Text = getBetween(r6dbString, "\"emea\":{\"mmr\":" + decRawMMR + ",\"rank\":", "},\"ncsa\"");
                        }

                        if (cboRegion.SelectedIndex == 2)
                        {
                            txtRank.Text = getBetween(r6dbString, "\"apac\":{\"mmr\":" + decRawMMR + ",\"rank\":", "},\"emea\"");
                        }

                        MMRToRank(txtRank);

                        btnUplay.Enabled = true;
                    }
                    catch //r6db
                    {
                        MessageBox.Show("Unable to find aliases or MMR. Did you input the correct username?", "Error r6db.com");
                        btnUplay.Enabled = false;
                    }

                    if (blStatsFail == true)
                    {
                        blStatsFail = false;
                        lstAliases.Items.Clear();
                    }

                }
                catch //everything
                {
                    MessageBox.Show("Unable to find username in databases.", "Error username");
                }
            }
        }

        //Compare Teams

        //Score

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (cboRegionTeam.SelectedIndex == -1)
            {
                cboRegionTeam.SelectedIndex = 0;
            }


            //Team 1
            RankedStats(txtT1P1, txtT1P1KD, txtT1P1WL, txtT1P1MMR, txtT1P1R, cboRegionTeam, "Team 1, Player 1, ");
            RankedStats(txtT1P2, txtT1P2KD, txtT1P2WL, txtT1P2MMR, txtT1P2R, cboRegionTeam, "Team 1, Player 2, ");
            RankedStats(txtT1P3, txtT1P3KD, txtT1P3WL, txtT1P3MMR, txtT1P3R, cboRegionTeam, "Team 1, Player 3, ");
            RankedStats(txtT1P4, txtT1P4KD, txtT1P4WL, txtT1P4MMR, txtT1P4R, cboRegionTeam, "Team 1, Player 4, ");
            RankedStats(txtT1P5, txtT1P5KD, txtT1P5WL, txtT1P5MMR, txtT1P5R, cboRegionTeam, "Team 1, Player 5, ");

            //Team 2
            RankedStats(txtT2P1, txtT2P1KD, txtT2P1WL, txtT2P1MMR, txtT2P1R, cboRegionTeam, "Team 2, Player 1, ");
            RankedStats(txtT2P2, txtT2P2KD, txtT2P2WL, txtT2P2MMR, txtT2P2R, cboRegionTeam, "Team 2, Player 2, ");
            RankedStats(txtT2P3, txtT2P3KD, txtT2P3WL, txtT2P3MMR, txtT2P3R, cboRegionTeam, "Team 2, Player 3, ");
            RankedStats(txtT2P4, txtT2P4KD, txtT2P4WL, txtT2P4MMR, txtT2P4R, cboRegionTeam, "Team 2, Player 4, ");
            RankedStats(txtT2P5, txtT2P5KD, txtT2P5WL, txtT2P5MMR, txtT2P5R, cboRegionTeam, "Team 2, Player 5, ");


            //Team 1 score

            decimal decT1Score = 0;

            try
            {
                decimal decT1KD = (Convert.ToDecimal(txtT1P1KD.Text) + Convert.ToDecimal(txtT1P2KD.Text) + Convert.ToDecimal(txtT1P3KD.Text) + Convert.ToDecimal(txtT1P4KD.Text) + Convert.ToDecimal(txtT1P5KD.Text)) * 135;
                decimal decT1WL = (Convert.ToDecimal(txtT1P1WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT1P2WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT1P3WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT1P4WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT1P5WL.Text.Replace("%", ""))) * .4m;
                decimal decT1MMR = 0;
                try
                {
                    decT1MMR = (Convert.ToDecimal(txtT1P1MMR.Text) + Convert.ToDecimal(txtT1P2MMR.Text) + Convert.ToDecimal(txtT1P3MMR.Text) + Convert.ToDecimal(txtT1P4MMR.Text) + Convert.ToDecimal(txtT1P5MMR.Text)) * .01337m;
                }
                catch { }
                decT1Score = decT1KD + decT1WL + decT1MMR;
            }
            catch
            {
                MessageBox.Show("Error in calculating Team 1 Chances", "Team 1 Error");
            }

            //Team 2 Score

            decimal decT2Score = 0;

            try
            {
                decimal decT2KD = (Convert.ToDecimal(txtT2P1KD.Text) + Convert.ToDecimal(txtT2P2KD.Text) + Convert.ToDecimal(txtT2P3KD.Text) + Convert.ToDecimal(txtT2P4KD.Text) + Convert.ToDecimal(txtT2P5KD.Text)) * 135;
                decimal decT2WL = (Convert.ToDecimal(txtT2P1WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT2P2WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT2P3WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT2P4WL.Text.Replace("%", "")) + Convert.ToDecimal(txtT2P5WL.Text.Replace("%", ""))) *.4m;
                decimal decT2MMR = 0;
                try
                {
                    decT2MMR = (Convert.ToDecimal(txtT2P1MMR.Text) + Convert.ToDecimal(txtT2P2MMR.Text) + Convert.ToDecimal(txtT2P3MMR.Text) + Convert.ToDecimal(txtT2P4MMR.Text) + Convert.ToDecimal(txtT2P5MMR.Text)) * .01337m;
                }
                catch
                { }
                decT2Score = decT2KD + decT2WL + decT2MMR;
            }
            catch
            {
                MessageBox.Show("Error in calculating Team 2 Chances", "Team 2 Error");
            }

            //Team Win percentages

            decimal decT1Percentage = 0;
            decimal decT2Percentage = 0;

            try
            {
                decT1Percentage = (decT1Score / decT2Score); //1.5
                decT2Percentage = (decT2Score / decT1Score); //.66
            }
            catch
            {

            }

            if (decT1Score < decT2Score)
            {
                decT1Percentage = Math.Round((((decT1Percentage - 1) * 100) + 50),2);
                decT2Percentage = 100 - decT1Percentage;
            }
            else if (decT1Score > decT2Score)
            {
                decT2Percentage = Math.Round((((decT2Percentage - 1) * 100) + 50),2);
                decT1Percentage = 100 - decT2Percentage;
            }
            else
            {
                decT1Percentage = 50;
                decT2Percentage = 50;
            }

            

            //Change back colors
            if (decT1Score > decT2Score)
            {
                grpT1.BackColor = Color.LimeGreen;
                grpT2.BackColor = Color.Red;
            }
            else if (decT2Score == decT1Score)
            {
                grpT1.BackColor = Color.LimeGreen;
                grpT2.BackColor = Color.LimeGreen;
            }
            else
            {
                grpT1.BackColor = Color.Red;
                grpT2.BackColor = Color.LimeGreen;
            }

            //Show win percentage
            grpT1.Text = "Team 1 - " + decT1Percentage + "% Chance of Winning";
            grpT2.Text = "Team 2 - " + decT2Percentage + "% Chance of Winning";

        }

        //Compare Stats
        private void btnCompare_Click(object sender, EventArgs e)
        {
            int intUser1NumRank = 0;
            int intUser2NumRank = 0;
            if (cboRegionCompare.SelectedIndex == -1)
            {
                cboRegionCompare.SelectedIndex = 0;
            }

            //User 1
            if (!string.IsNullOrWhiteSpace(txtUser1.Text))
            {
                try
                {
                    //Load ranked
                    string ranked1Url = "https://r6stats.com/stats/uplay/" + txtUser1.Text + "/ranked";
                    HtmlWeb ranked1Web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument ranked1Doc = ranked1Web.Load(ranked1Url);

                    //Load r6db
                    string db1Url = "https://r6db.com/api/v2/players?name=" + txtUser1.Text + "&exact=0";
                    var db1Request = (HttpWebRequest)WebRequest.Create(db1Url);
                    db1Request.ContentType = "application/x-www-form-urlencoded";

                    db1Request.Headers.Add("x-app-id", "r6db-frontend");

                    WebResponse r6dbResponse = db1Request.GetResponse();

                    Stream db1Stream = r6dbResponse.GetResponseStream();

                    StreamReader r6dbreader = new StreamReader(db1Stream);

                    String db1String = r6dbreader.ReadToEnd();

                    r6dbResponse.Close();


                    //Stats
                    txtUser1KD.Text = ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[3]/div/div[2]")[0].InnerText;
                    txtUser1WL.Text = ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[2]/div/div[2]")[0].InnerText;
                    txtUser1PT.Text = ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[4]/div/div[2]")[0].InnerText;
                    txtUser1Acc.Text = ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[2]/td[2]")[0].InnerText;
                    txtUser1Head.Text = Convert.ToString(Decimal.Parse(ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[3]/td[2]")[0].InnerText.Replace("%", "")) * 10m) + "%";
                    txtUser1Level.Text = ranked1Doc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[4]/div/div[2]")[0].InnerText;

                    decimal decRawMMR1 = 0;
                    if (cboRegionCompare.SelectedIndex == 0)
                    {
                        decRawMMR1 = Convert.ToDecimal(getBetween(db1String, "\"ncsa\":{\"mmr\":", ",\""));
                    }
                    if (cboRegionCompare.SelectedIndex == 1)
                    {
                        decRawMMR1 = Convert.ToDecimal(getBetween(db1String, "\"emea\":{\"mmr\":", ",\""));
                    }
                    if (cboRegionCompare.SelectedIndex == 2)
                    {
                        decRawMMR1 = Convert.ToDecimal(getBetween(db1String, "\"apac\":{\"mmr\":", ",\""));
                    }

                    txtUser1MMR.Text = Convert.ToString(Math.Round(Convert.ToDecimal(decRawMMR1)));

                    if (cboRegionCompare.SelectedIndex == 0)
                    {
                        txtUser1Rank.Text = getBetween(db1String, "\"ncsa\":{\"mmr\":" + decRawMMR1 + ",\"rank\":", "}},\"updated_at\"");
                    }

                    if (cboRegionCompare.SelectedIndex == 1)
                    {
                        txtUser1Rank.Text = getBetween(db1String, "\"emea\":{\"mmr\":" + decRawMMR1 + ",\"rank\":", "},\"ncsa\"");
                    }

                    if (cboRegionCompare.SelectedIndex == 2)
                    {
                        txtUser1Rank.Text = getBetween(db1String, "\"apac\":{\"mmr\":" + decRawMMR1 + ",\"rank\":", "},\"emea\"");
                    }


                    intUser1NumRank = Convert.ToInt32(txtUser1Rank.Text);
                    MMRToRank(txtUser1Rank);

                }
                catch
                {
                    MessageBox.Show("Username 1 not valid.", "User 1 Error");
                }
            }
            //User 2
            if (!string.IsNullOrWhiteSpace(txtUser2.Text))
            {
                try
                {
                    //Load ranked
                    string ranked2Url = "https://r6stats.com/stats/uplay/" + txtUser2.Text + "/ranked";
                    HtmlWeb ranked2Web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument ranked2Doc = ranked2Web.Load(ranked2Url);

                    //Load r6db
                    string db2Url = "https://r6db.com/api/v2/players?name=" + txtUser2.Text + "&exact=0";
                    var db2Request = (HttpWebRequest)WebRequest.Create(db2Url);
                    db2Request.ContentType = "application/x-www-form-urlencoded";

                    db2Request.Headers.Add("x-app-id", "r6db-frontend");

                    WebResponse r6dbResponse = db2Request.GetResponse();

                    Stream db2Stream = r6dbResponse.GetResponseStream();

                    StreamReader r6dbreader = new StreamReader(db2Stream);

                    String db2String = r6dbreader.ReadToEnd();

                    r6dbResponse.Close();


                    //Stats
                    txtUser2KD.Text = ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[3]/div/div[2]")[0].InnerText;
                    txtUser2WL.Text = ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[2]/div/div[2]")[0].InnerText;
                    txtUser2PT.Text = ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[4]/div/div[2]")[0].InnerText;
                    txtUser2Acc.Text = ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[2]/td[2]")[0].InnerText;
                    txtUser2Head.Text = Convert.ToString(Decimal.Parse(ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div[2]/div/table/tbody/tr[3]/td[2]")[0].InnerText.Replace("%", "")) * 10m) + "%";
                    txtUser2Level.Text = ranked2Doc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[4]/div/div[2]")[0].InnerText;

                    decimal decRawMMR2 = 0;
                    if (cboRegionCompare.SelectedIndex == 0)
                    {
                        decRawMMR2 = Convert.ToDecimal(getBetween(db2String, "\"ncsa\":{\"mmr\":", ",\""));
                    }
                    if (cboRegionCompare.SelectedIndex == 1)
                    {
                        decRawMMR2 = Convert.ToDecimal(getBetween(db2String, "\"emea\":{\"mmr\":", ",\""));
                    }
                    if (cboRegionCompare.SelectedIndex == 2)
                    {
                        decRawMMR2 = Convert.ToDecimal(getBetween(db2String, "\"apac\":{\"mmr\":", ",\""));
                    }

                    txtUser2MMR.Text = Convert.ToString(Math.Round(Convert.ToDecimal(decRawMMR2)));

                    if (cboRegionCompare.SelectedIndex == 0)
                    {
                        txtUser2Rank.Text = getBetween(db2String, "\"ncsa\":{\"mmr\":" + decRawMMR2 + ",\"rank\":", "}},\"updated_at\"");
                    }

                    if (cboRegionCompare.SelectedIndex == 1)
                    {
                        txtUser2Rank.Text = getBetween(db2String, "\"emea\":{\"mmr\":" + decRawMMR2 + ",\"rank\":", "},\"ncsa\"");
                    }

                    if (cboRegionCompare.SelectedIndex == 2)
                    {
                        txtUser2Rank.Text = getBetween(db2String, "\"apac\":{\"mmr\":" + decRawMMR2 + ",\"rank\":", "},\"emea\"");
                    }

                    intUser2NumRank = Convert.ToInt32(txtUser2Rank.Text);
                    MMRToRank(txtUser2Rank);
                }
                catch
                {
                    MessageBox.Show("Username 2 not valid.", "User 2 Error");
                }
            }

            try
            {
                //Compare levels
                if (Convert.ToInt32(txtUser1Level.Text) > Convert.ToInt32(txtUser2Level.Text))
                {
                    txtUser1Level.BackColor = Color.LimeGreen;
                    txtUser2Level.BackColor = Color.Red;
                }
                else if (Convert.ToInt32(txtUser1Level.Text) == Convert.ToInt32(txtUser2Level.Text))
                {
                    txtUser1Level.BackColor = Color.LimeGreen;
                    txtUser2Level.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1Level.BackColor = Color.Red;
                    txtUser2Level.BackColor = Color.LimeGreen;
                }
                //Compare accuracy
                if (Convert.ToDecimal(txtUser1Acc.Text.Replace("%", "")) > Convert.ToDecimal(txtUser2Acc.Text.Replace("%", "")))
                {
                    txtUser1Acc.BackColor = Color.LimeGreen;
                    txtUser2Acc.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1Acc.Text.Replace("%", "")) == Convert.ToDecimal(txtUser2Acc.Text.Replace("%", "")))
                {
                    txtUser1Acc.BackColor = Color.LimeGreen;
                    txtUser2Acc.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1Acc.BackColor = Color.Red;
                    txtUser2Acc.BackColor = Color.LimeGreen;
                }
                //Compare headshots
                if (Convert.ToDecimal(txtUser1Head.Text.Replace("%", "")) > Convert.ToDecimal(txtUser2Head.Text.Replace("%", "")))
                {
                    txtUser1Head.BackColor = Color.LimeGreen;
                    txtUser2Head.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1Head.Text.Replace("%", "")) == Convert.ToDecimal(txtUser2Head.Text.Replace("%", "")))
                {
                    txtUser1Head.BackColor = Color.LimeGreen;
                    txtUser2Head.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1Head.BackColor = Color.Red;
                    txtUser2Head.BackColor = Color.LimeGreen;
                }
                //Compare k/d
                if (Convert.ToDecimal(txtUser1KD.Text) > Convert.ToDecimal(txtUser2KD.Text))
                {
                    txtUser1KD.BackColor = Color.LimeGreen;
                    txtUser2KD.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1KD.Text) == Convert.ToDecimal(txtUser2KD.Text))
                {
                    txtUser1KD.BackColor = Color.LimeGreen;
                    txtUser2KD.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1KD.BackColor = Color.Red;
                    txtUser2KD.BackColor = Color.LimeGreen;
                }
                //Compare win %
                if (Convert.ToDecimal(txtUser1WL.Text.Replace("%", "")) > Convert.ToDecimal(txtUser2WL.Text.Replace("%", "")))
                {
                    txtUser1WL.BackColor = Color.LimeGreen;
                    txtUser2WL.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1WL.Text.Replace("%", "")) == Convert.ToDecimal(txtUser2WL.Text.Replace("%", "")))
                {
                    txtUser1WL.BackColor = Color.LimeGreen;
                    txtUser2WL.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1WL.BackColor = Color.Red;
                    txtUser2WL.BackColor = Color.LimeGreen;
                }
                //Compare MMR
                if (Convert.ToDecimal(txtUser1MMR.Text) > Convert.ToDecimal(txtUser2MMR.Text))
                {
                    txtUser1MMR.BackColor = Color.LimeGreen;
                    txtUser2MMR.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1MMR.Text) == Convert.ToDecimal(txtUser2MMR.Text))
                {
                    txtUser1MMR.BackColor = Color.LimeGreen;
                    txtUser2MMR.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1MMR.BackColor = Color.Red;
                    txtUser2MMR.BackColor = Color.LimeGreen;
                }
                //Compare play time
                if (Convert.ToDecimal(txtUser1PT.Text.Replace("H", "")) > Convert.ToDecimal(txtUser2PT.Text.Replace("H", "")))
                {
                    txtUser1PT.BackColor = Color.LimeGreen;
                    txtUser2PT.BackColor = Color.Red;
                }
                else if (Convert.ToDecimal(txtUser1PT.Text.Replace("H", "")) == Convert.ToDecimal(txtUser2PT.Text.Replace("H", "")))
                {
                    txtUser1PT.BackColor = Color.LimeGreen;
                    txtUser2PT.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1PT.BackColor = Color.Red;
                    txtUser2PT.BackColor = Color.LimeGreen;
                //Rank
                }
                if (intUser1NumRank > intUser2NumRank)
                {
                    txtUser1Rank.BackColor = Color.LimeGreen;
                    txtUser2Rank.BackColor = Color.Red;
                }
                else if (intUser1NumRank == intUser2NumRank)
                {
                    txtUser1Rank.BackColor = Color.LimeGreen;
                    txtUser2Rank.BackColor = Color.LimeGreen;
                }
                else
                {
                    txtUser1Rank.BackColor = Color.Red;
                    txtUser2Rank.BackColor = Color.LimeGreen;
                }
            }
            catch
            {
                MessageBox.Show("Username(s) are invalid", "Error");
            }


        }

        //search string
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        //create list
        public static string getNames(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                string strResult = strSource.Substring(Start, End - Start);
                return strResult;
            }
            else
            {
                return "";
            }
        }

        //Show Names
        public void showNames(string strSource)
        {
            var count = Regex.Matches(Regex.Escape(strSource), "\"name\":\"").Count - 1;

            for (int i = 0; i < count; i++)
            {
                lstAliases.Items.Insert(0, getNames(strSource, "\"name\":\"", "\"}"));
                if (strSource.Contains("\"}"))
                {
                    string strEnd = "\"}";
                    strSource = strSource.Substring(strSource.IndexOf(strEnd) + 1);
                }
            }
        }

        //View account on uplay
        private void btnUplay_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://game-rainbow6.ubi.com/en-us/uplay/player-statistics/" + txtUplayID.Text + "/multiplayer");
        }

        //Rank method
        public string MMRToRank (TextBox Textbox)
        {
            switch (Convert.ToInt32(Textbox.Text))
            {
                case 1:
                    Textbox.Text = "Copper 1";
                    break;
                case 2:
                    Textbox.Text = "Copper 2";
                    break;
                case 3:
                    Textbox.Text = "Copper 3";
                    break;
                case 4:
                    Textbox.Text = "Copper 4";
                    break;
                case 5:
                    Textbox.Text = "Bronze 1";
                    break;
                case 6:
                    Textbox.Text = "Bronze 2";
                    break;
                case 7:
                    Textbox.Text = "Bronze 3";
                    break;
                case 8:
                    Textbox.Text = "Bronze 4";
                    break;
                case 9:
                    Textbox.Text = "Silver 1";
                    break;
                case 10:
                    Textbox.Text = "Silver 2";
                    break;
                case 11:
                    Textbox.Text = "Silver 3";
                    break;
                case 12:
                    Textbox.Text = "Silver 4";
                    break;
                case 13:
                    Textbox.Text = "Gold 1";
                    break;
                case 14:
                    Textbox.Text = "Gold 2";
                    break;
                case 15:
                    Textbox.Text = "Gold 3";
                    break;
                case 16:
                    Textbox.Text = "Gold 4";
                    break;
                case 17:
                    Textbox.Text = "Platinum 1";
                    break;
                case 18:
                    Textbox.Text = "Platinum 2";
                    break;
                case 19:
                    Textbox.Text = "Platinum 3";
                    break;
                case 20:
                    Textbox.Text = "Diamond";
                    break;
                default:
                    Textbox.Text = "Unranked";
                    break;
            }
            return Textbox.Text;
        }

        //Method to calculate each person's ranked stats
        public void RankedStats(TextBox Username, TextBox KD, TextBox WL, TextBox MMR, TextBox RANK, ComboBox Region, String TeamPlayer)
        {
            if (!string.IsNullOrWhiteSpace(Username.Text))
            {
                try
                {
                    string rankedUrl = "https://r6stats.com/stats/uplay/" + Username.Text + "/ranked";
                    HtmlWeb rankedWeb = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument rankedDoc = rankedWeb.Load(rankedUrl);

                    //Load r6db
                    string dbUrl = "https://r6db.com/api/v2/players?name=" + Username.Text + "&exact=0";
                    var dbRequest = (HttpWebRequest)WebRequest.Create(dbUrl);
                    dbRequest.ContentType = "application/x-www-form-urlencoded";

                    dbRequest.Headers.Add("x-app-id", "r6db-frontend");

                    WebResponse r6dbResponse = dbRequest.GetResponse();

                    Stream dbStream = r6dbResponse.GetResponseStream();

                    StreamReader r6dbreader = new StreamReader(dbStream);

                    String dbString = r6dbreader.ReadToEnd();

                    r6dbResponse.Close();



                    //Stats
                    KD.Text = Convert.ToString(Math.Round(Convert.ToDecimal(rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[2]/div[3]/div/div[2]")[0].InnerText), 2));
                    WL.Text = rankedDoc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[2]/div/div[2]")[0].InnerText;
                   

                    decimal RawMMR = 0;
                    if (Region.SelectedIndex == 0)
                    {
                        RawMMR = Convert.ToDecimal(getBetween(dbString, "\"ncsa\":{\"mmr\":", ",\""));
                    }
                    if (Region.SelectedIndex == 1)
                    {
                        RawMMR = Convert.ToDecimal(getBetween(dbString, "\"emea\":{\"mmr\":", ",\""));
                    }
                    if (Region.SelectedIndex == 2)
                    {
                        RawMMR = Convert.ToDecimal(getBetween(dbString, "\"apac\":{\"mmr\":", ",\""));
                    }

                    MMR.Text = Convert.ToString(Math.Round(Convert.ToDecimal(RawMMR)));

                    if (Region.SelectedIndex == 0)
                    {
                        RANK.Text = getBetween(dbString, "\"ncsa\":{\"mmr\":" + RawMMR + ",\"rank\":", "}},\"updated_at\"");
                    }

                    if (Region.SelectedIndex == 1)
                    {
                        RANK.Text = getBetween(dbString, "\"emea\":{\"mmr\":" + RawMMR + ",\"rank\":", "},\"ncsa\"");
                    }

                    if (Region.SelectedIndex == 2)
                    {
                        RANK.Text = getBetween(dbString, "\"apac\":{\"mmr\":" + RawMMR + ",\"rank\":", "},\"emea\"");
                    }


                    switch (Convert.ToInt32(RANK.Text))
                    {
                        case 1:
                            RANK.Text = "C1";
                            break;
                        case 2:
                            RANK.Text = "C2";
                            break;
                        case 3:
                            RANK.Text = "C3";
                            break;
                        case 4:
                            RANK.Text = "C4";
                            break;
                        case 5:
                            RANK.Text = "B1";
                            break;
                        case 6:
                            RANK.Text = "B2";
                            break;
                        case 7:
                            RANK.Text = "B3";
                            break;
                        case 8:
                            RANK.Text = "B4";
                            break;
                        case 9:
                            RANK.Text = "S1";
                            break;
                        case 10:
                            RANK.Text = "S2";
                            break;
                        case 11:
                            RANK.Text = "S3";
                            break;
                        case 12:
                            RANK.Text = "S4";
                            break;
                        case 13:
                            RANK.Text = "G1";
                            break;
                        case 14:
                            RANK.Text = "G2";
                            break;
                        case 15:
                            RANK.Text = "G3";
                            break;
                        case 16:
                            RANK.Text = "G4";
                            break;
                        case 17:
                            RANK.Text = "P1";
                            break;
                        case 18:
                            RANK.Text = "P2";
                            break;
                        case 19:
                            RANK.Text = "P3";
                            break;
                        case 20:
                            RANK.Text = "D";
                            break;
                        default:
                            RANK.Text = "N/A";
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show(TeamPlayer + "is not a valid username", "Invalid username");
                    KD.Clear();
                    WL.Clear();
                    MMR.Clear();
                    RANK.Clear();
                }
            }
        }


        //Change accept button
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    AcceptButton = button1;
                    break;
                case 1:
                    AcceptButton = btnCompare;
                    break;
                case 2:
                    AcceptButton = btnCalculate;
                    break;
            }
        }
    }
}

