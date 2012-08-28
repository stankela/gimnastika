using System;
using System.Drawing;
using System.Data;
using Gimnastika.UI;

namespace Gimnastika.Report
{
	/// <summary>
	/// Summary description for Izvestaj.
	/// </summary>
	public class Izvestaj
	{
		private DataSet dataset = new DataSet();

		private string documentName;
		private string title;
		private string subTitle = "";
		private float relHeight = 24.5f;
		private float relHeaderHeight = 2.7f;
		protected float relWidth = 17.2f;
		private float relPictureWidth = 4.7f;

		private StringFormat titleFormat;
		private StringFormat subTitleFormat;
		private StringFormat dateFormat;

		private Font titleFont;
		private Font subTitleFont;
		private Font pageNumFont;
		private Font sokDruVojFont;
		private Font adresaFont;
		protected Brush blackBrush;

		private PrintPreviewForm prevForm;

		public Izvestaj()
		{

        }

		public void init()
		{
			createAndFillTables();
			createFormats();
		}

        protected PrintPreviewForm getPreviewForm()
		{
			return prevForm;
		}

        public void setPreviewForm(PrintPreviewForm prevForm)
		{
            this.prevForm = prevForm;
		}

		public DataSet getDataSet()
		{
			return dataset;
		}

		public string getTitle()
		{
			return title;
		}

		public void setTitle(string title)
		{
			this.title = title;
		}

		public string getSubTitle()
		{
			return subTitle;
		}

		public void setSubTitle(string subTitle)
		{
			this.subTitle = subTitle;
		}

		public string getDocumentName()
		{
			return documentName;
		}

		public void setDocumentName(string docName)
		{
			this.documentName = docName;
		}

		protected virtual void createFormats()
		{
			titleFormat = new StringFormat();
			titleFormat.Alignment = StringAlignment.Center;
			titleFormat.LineAlignment = StringAlignment.Near;

			subTitleFormat = new StringFormat();
			subTitleFormat.Alignment = StringAlignment.Center;
			subTitleFormat.LineAlignment = StringAlignment.Far;

			dateFormat = new StringFormat();
			dateFormat.Alignment = StringAlignment.Far;
			dateFormat.LineAlignment = StringAlignment.Near;
		}

		protected virtual void createAndFillTables()
		{
			dataset.Clear();
			dataset = new DataSet();
			createPageLayoutTable();
			fillItemsTable();
			fillGroupsTable();
		}

		protected virtual void fillItemsTable()
		{

		}

		protected virtual void fillGroupsTable()
		{

		}
		
		protected virtual void createPageLayoutTable()
		{
			DataTable tbl = dataset.Tables.Add("Page Layout");
			tbl.Columns.Add("Page", typeof(int));
			tbl.Columns.Add("Group", typeof(int));
			tbl.Columns.Add("Start Rec", typeof(int));
			tbl.Columns.Add("Num Rec", typeof(int));
			tbl.Columns.Add("Y", typeof(float));
			tbl.Columns.Add("Header", typeof(bool));
			tbl.PrimaryKey = new DataColumn[] { tbl.Columns["Page"], tbl.Columns["Group"] };
		}

		public virtual void BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			createFonts();
			//e.Cancel = true;
		}

		protected virtual void createFonts()
		{
			titleFont = new Font("Tahoma", 14, FontStyle.Bold);
			subTitleFont = new Font("Tahoma", 10, FontStyle.Bold);
			pageNumFont = new Font("Arial", 8);
			sokDruVojFont = new Font("Arial Narrow", 8);
			adresaFont = new Font("Arial Narrow", 7);
			blackBrush = Brushes.Black;
		}

		protected virtual void releaseFonts()
		{
			titleFont.Dispose();
			subTitleFont.Dispose();
			pageNumFont.Dispose();
			sokDruVojFont.Dispose();
			adresaFont.Dispose();
			// blackBrush.Dispose();  // daje gresku
		}
		
		public virtual void EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			releaseFonts();
		}

		public virtual float getHeaderHeight(Graphics g, RectangleF marginBounds, int pageNum)
		{
			return relHeaderHeight / relHeight * marginBounds.Height;
		}

		public virtual void drawHeader(Graphics g, RectangleF headerBounds, int pageNum)
		{
			float pictureWidth = relPictureWidth / relWidth * headerBounds.Width;
			float pictureHeight = headerBounds.Height * 0.8f;
			RectangleF pictureBounds = new RectangleF(headerBounds.X, headerBounds.Y, pictureWidth, pictureHeight);
			drawSokoWithCaption(g, pictureBounds);

			float lineOffset = headerBounds.Height * 0.9f;
			using(Pen pen = new Pen(Color.Black, 1/72f * 2f))
			{
				g.DrawLine(pen, new PointF(headerBounds.X, headerBounds.Y + lineOffset),
					new PointF(headerBounds.X + headerBounds.Width, headerBounds.Y + lineOffset));
			}

			float titleHeight = titleFont.GetHeight(g);
			if (subTitle != "")
				titleHeight += subTitleFont.GetHeight(g) * 1.5f;
			float titleY = headerBounds.Y + (headerBounds.Height - titleHeight) / 3;
			RectangleF titleBounds = new RectangleF(headerBounds.X, titleY, 
				headerBounds.Width, titleHeight);
				
			g.DrawString(title, titleFont, blackBrush, titleBounds, titleFormat); 
			if (subTitle != "")
				g.DrawString(subTitle, subTitleFont, blackBrush, titleBounds, subTitleFormat); 

			String page = "Strana";
			String from = "od";
            string datum = DateUtilities.serbianDateStr(DateTime.Now, '.');
			string vreme = DateTime.Now.ToLongTimeString();
			g.DrawString(datum + " " + vreme, pageNumFont, blackBrush, 
				headerBounds.Right, headerBounds.Top, dateFormat); 
			g.DrawString(String.Format("{0} {1} {2} {3}", page, pageNum, from, prevForm.getTotalPages()), pageNumFont, blackBrush, 
				headerBounds.Right, headerBounds.Top + pageNumFont.GetHeight(g) * 1.5f, dateFormat); 
		}

		private void drawSokoWithCaption(Graphics g, RectangleF pictureBounds)
		{
            //Image sokoImage = Image.FromFile(@"..\..\soko.bmp");
			string sokDruVoj = "SOKOLSKO DRUSTVO \"VOJVODINA\"";
			string adresa = "Ignjata Pavlasa 2-4, 21000, Novi Sad, Srbija";

			float ySokDruVoj = pictureBounds.Y + 0.7f * pictureBounds.Height;
			float yAdresa = pictureBounds.Y + 0.85f * pictureBounds.Height;
			RectangleF sokDruVojBounds = new RectangleF(pictureBounds.X, ySokDruVoj, 
				pictureBounds.Width, yAdresa - ySokDruVoj);
			RectangleF adresaBounds = new RectangleF(pictureBounds.X, yAdresa, 
				pictureBounds.Width, pictureBounds.Y + pictureBounds.Height - yAdresa);

			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				//			g.DrawRectangle(pen, pictureBounds.X, pictureBounds.Y, pictureBounds.Width, pictureBounds.Height);
			}
			float sokoHeight = 0.7f * pictureBounds.Height;
			float sokoWidth = sokoHeight;
			float sokoX = pictureBounds.X + (pictureBounds.Width - sokoWidth) / 2;
			RectangleF sokoBounds = new RectangleF(sokoX, pictureBounds.Y, sokoWidth, sokoHeight);
			//g.DrawImage(sokoImage, sokoBounds);
			g.DrawString(sokDruVoj, sokDruVojFont, blackBrush, sokDruVojBounds, titleFormat); 
			g.DrawString(adresa, adresaFont, blackBrush, adresaBounds, titleFormat); 
		}

		public virtual void drawContent(Graphics g, RectangleF contentBounds, int pageNum)
		{

		}
		
		protected virtual void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{

		}

		public virtual void setupContent(Graphics g, RectangleF contentBounds)
		{

		}

		public void QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
		{
			// Set margins to .5" all the way around
			//e.PageSettings.Margins = new Margins(50, 50, 50, 50);
		}

		public void clearDataSet()
		{
			dataset.Clear();
		}

	}
}
