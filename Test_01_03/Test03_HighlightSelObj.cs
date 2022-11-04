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
    public class Test03_HighlightSelObj : IExternalCommand
    {
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection selection = uidoc.Selection;

			List<ElementId> selectId = new List<ElementId>();

			bool flag = true;
			//TransactionManager.Instance.ForceCloseTransaction()
			TaskDialog.Show("Selection", "Pick elements in the desired order (re-select to Remove), hit ESC to stop picking.");
			TransactionGroup tg = new TransactionGroup(doc, "tgSelection");
			tg.Start();
			highlight hi = new highlight(doc, uidoc);

			while (flag)
				try
				{
					Reference refer = selection.PickObject(ObjectType.Element, "Pick elements in the desired order (re-select to Remove), hit ESC to stop picking.");
					ElementId e_id = refer.ElementId;
					if (!selectId.Contains(e_id))
					{
						hi.overridecolor(e_id,false);
						selectId.Add(e_id);
					}
					else
					{
						hi.overridecolor(e_id, true);
						selectId.pop(selectId.index(e_id));
					}
				}


				catch (Exception ex)
				{
					flag = false;
					break;
				}
			tg.RollBack();
			elemenSelect = [doc.GetElement(xId) for xId in selectId];
			
			

			MessageBox.Show("多個管件已被選擇");
			return Result.Succeeded;

		}
		public class highlight 
		{
			Transaction t = null;
			Document d = null;
			Autodesk.Revit.DB.View view = null;
			bool reset = false;
			Color color_rgb = new Color(30, 144, 255);
			OverrideGraphicSettings gSettings = new OverrideGraphicSettings();
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
				t.Start();
				reset = resetting;
				if (reset != false)
				{
					gSettings.SetSurfaceForegroundPatternColor(color_rgb);
					gSettings.SetProjectionLineColor(color_rgb);
					gSettings.SetCutLineColor(color_rgb);
					gSettings.SetCutForegroundPatternColor(color_rgb);
					gSettings.SetProjectionLineWeight(8);
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
}
