﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneSide
{
    class WindowChangeHelper
    {
        public void torestart()
        {
            AppPanel.mainFrame.Visible = false;
            AppPanel.singleFrame.Visible = false;
            AppPanel.messageFrame.Visible = true;

            AppPanel.messageFrame.textbox.Text = "ERROR!!\r\n Excelファイルの読み込みに失敗した。Excelファイルの利用を確認の上、アプリを再起動してください。" ;
            AppPanel.messageFrame.button.Visible = false;
            if (Data.errorline == 0)
            {
                DialogResult dr = MessageBox.Show("ファイルが開けない", "ER", MessageBoxButtons.OK);
            }
            else
            {
                char ch = 'A';
                while (Data.errorcolum != 0) { ch++; Data.errorcolum--; }

                DialogResult dr = MessageBox.Show("エラー場所: "+ Convert.ToString(Data.errorline+1)+ch , "ER", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        public void toError(string errorMessage, string buttonText)
        {
            showmessage("ERROR!!\r\n" + errorMessage);
            AppPanel.messageFrame.button.Text = buttonText;
        }

        public void toError(string errorMessage)
        {
            toError(errorMessage, "確認");
        }


        public void toMessgae(string messgae)
        {
            showmessage("手順:\r\n" + messgae);
            AppPanel.messageFrame.button.Text = "確認";
        }

        public void toMessage(string message, string buttonText)
        {
            showmessage(message);
            AppPanel.messageFrame.button.Text = buttonText;
        }
        public void showmessage(string message)
        {
            AppPanel.mainFrame.Visible = false;
            AppPanel.singleFrame.Visible = false;
            AppPanel.messageFrame.Visible = true;

            AppPanel.messageFrame.textbox.Text = message;
        }

        public void toplural(int indexfrom, int indexto, int indexcomeon)
        {
            AppPanel.boxTable.Visible = false;
            AppPanel.pluralTable.Visible = true;
            toMessgae("この商品は複数口の注文、詳細は右側を確認してください。");

            DealHelper dealHelper = new DealHelper();

            //複数口専用のboxの名前を決める
            if (Data.dbPluraName[indexfrom] == "")
                dealHelper.usePluralBox(indexfrom, indexto);//ないなら、新しいものから一つ使用する。

            bool finwithoutstore = dealHelper.finwithoutthree(indexfrom, indexto);
            bool fin = dealHelper.showPluralBox(indexfrom, indexto);

            AppPanel.pluralTable.dataGridView1.CurrentCell = AppPanel.pluralTable.dataGridView1[1, indexcomeon-indexfrom];
            Console.WriteLine("from: " + Convert.ToString(indexfrom));
            Console.WriteLine("to:   "+ Convert.ToString(indexto));
            Console.WriteLine("index: "+Convert.ToString(indexcomeon));

            if (finwithoutstore)
            {
                if (fin)
                {
                    if(Data.dbSendway[indexfrom] != "air" && Data.dbSendway[indexfrom] != "*")
                    {
                        toMessgae("複数の注文、" +
                            Data.dbPluraName[indexfrom] + "内の商品がすべてそろいました" +
                            "特殊発送方法：【" + Data.dbSendway[indexfrom] + "】");
                        for(int idx = indexfrom; idx <= indexto; idx++)
                        {
                            Data.dbBoxNo[idx] = "special";
                        }
                        return;
                    }

                    toMessgae("この商品は複数口の注文、" +
                        "この商品で" + Data.dbPluraName[indexfrom] + "と割り振られた商品がすべて揃いましたので、" +
                        "全部まとめてbox" + (Data.boxName[Data.SIN] + Data.SIN*10000) + "入れて出荷してください。");
                    dealHelper.putintobox(indexfrom, indexto);
                }
                else
                {
                    toMessgae("この商品は複数口の注文、\r\n" +
                        "残り商品は在庫品になります！、三階へ送ってね.\r\n" +
                        "三階へ三階へ三階へ");
                }
                
            }
        }

        public void tofinishbox()
        //public void tofinishbox(LinkedList<int> i)
        {
            toMessgae(Data.GOODSMAXNUM + "件商品がそろいましたので、グリーンラベル、インヴォイスの印刷を開始します。");
            AppPanel.messageFrame.button.Text = "印刷";
        }

    }
}
