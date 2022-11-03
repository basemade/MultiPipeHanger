using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;


namespace Test_01_03
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Test03_HighlightSelObjSort : IExternalCommand
    {
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection selection = uidoc.Selection;
			List<ElementId> selectId = new List<ElementId>();
			//上面這行需比較List與Ilist的差異
			StringBuilder st = new StringBuilder();

			bool flag = true;
			//TransactionManager.Instance.ForceCloseTransaction()
			//just to make sure everything is closed down
			//上面這行查到的是說：
			//Dynamo's transaction handling is pretty poor for multiple documents,
			//so we'll need to force close every single transaction we open

			TaskDialog.Show("Selection", "Pick elements in the desired order (re-select to Remove), hit ESC to stop picking.");
			TransactionGroup tg = new TransactionGroup(doc, "tgSelection");
			tg.Start();
			

			while (flag == true)
				try
				{
					Reference refer = selection.PickObject(ObjectType.Element, "Pick elements in the desired order (re-select to Remove), hit ESC to stop picking.");
					ElementId e_id = refer.ElementId;
					if (!selectId.Contains(e_id))
					{
						highlight hi = new highlight(doc, uidoc);
						hi.overridecolor(e_id, true);
						selectId.Add(e_id);
					}
					else
					{
						highlight hi = new highlight(doc, uidoc);
						hi.overridecolor(e_id, false);
                        selectId.RemoveAt(selectId.IndexOf(e_id));
					}
				}

				catch (Autodesk.Revit.Exceptions.OperationCanceledException)
				{
					flag = false;
					break;
				}

			tg.RollBack();
			foreach (ElementId xId in selectId)
			{
				st.AppendLine("element.Id：" + xId);
			}

			
			MessageBox.Show(st.ToString());
			return Result.Succeeded;

		}
		public class highlight 
		{
			Transaction t = null;
			Document d = null;
			Autodesk.Revit.DB.View view = null;
			bool reset;
			Color color_rgb = new Color(30, 144, 255);
			OverrideGraphicSettings gSettings;
			UIDocument uidocument;

			public highlight(Document doc, UIDocument uidoc)
			{
				d = doc;
				t = new Transaction(doc, "Selection");
				view = d.ActiveView;
				uidocument = uidoc;
			}
			public void overridecolor(ElementId elementId, bool resetting)
			{
				gSettings = new OverrideGraphicSettings();
				t.Start();
				reset = resetting;

				
				if (reset != false)
				{
					gSettings.SetSurfaceBackgroundPatternColor(color_rgb);
					gSettings.SetSurfaceForegroundPatternColor(color_rgb);
					gSettings.SetProjectionLineColor(color_rgb);
					gSettings.SetCutLineColor(color_rgb);
					gSettings.SetCutForegroundPatternColor(color_rgb);
					gSettings.SetProjectionLineWeight(8);
					gSettings.SetSurfaceTransparency(80);
				}
				
				view.SetElementOverrides(elementId, gSettings);
				d.Regenerate();
				uidocument.RefreshActiveView();
				t.Commit();
				t.Dispose();
			}
		}


							
        
    }
}
