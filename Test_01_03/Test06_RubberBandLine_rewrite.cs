using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Windows; //Need reference: AdWindows dll, from C:\Program Files\Autodesk\Revit yyyy
using Microsoft.VisualBasic.CompilerServices;


namespace Test_01_03
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class Test06_RubberBandLine_rewrite : IExternalCommand
	//我們在寫的是類別庫，而Test06_ConceptRefresh這個Class，有利用IExternalCommand這個介面 進行方法的擴充
	//IExternalCommand是Revit API用戶透過外部命令来擴展功能的介面
	{
		private IntPtr _revit_window;
		private List<ElementId> _added_element_ids;

		public Test06_RubberBandLine_rewrite()
		{
			_added_element_ids = new List<ElementId>();
		}
		//Test06_ConceptRefresh這個Class的建構子

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		//Test06_ConceptRefresh這個Class在實作時，需要宣告IExternalCommand這個介面要求的方法Execute()
		//方法Execute()回傳型別為Result，所以這個方法最後會回傳狀態 ex: return Result.Succeeded;
		//方法Execute()並且需要宣告三種型別的參數ExternalCommandData, ref string , ElementSet
		//ExternalCommandData is a class contains reference to Application and View which are needed by external command.
		{
			UIApplication application = commandData.Application;
			//ExternalCommandData 這個類別含有一個property ".Application" 型別為UIApplication；所有的Revit的數據都可以透過這個參數直接或間接地取得
			UIDocument activeUIDocument = application.ActiveUIDocument;
			Application application2 = application.Application;
			//????
			Document document = activeUIDocument.Document;

			_revit_window = application.MainWindowHandle;


			FilteredElementCollector val = new FilteredElementCollector(document);

            //val.OfCategory((BuiltInCategory)(-2002000));//OST_DetailComponents = -2002000	//原寫法特別
            val.OfCategory(BuiltInCategory.OST_DetailComponents);
			//Applies an ElementCategoryFilter to the collector.
			//Return Value：This collector.

			val.OfClass(typeof(FamilySymbol));
			//Applies an ElementClassFilter to the collector.
			//Return Value：A handle to this collector.This is the same collector that has just been modified, returned so you can chain multiple calls together in one line.
			//Gettype()返回的是實例的type，而typeof返回的是類型的type
			//typeof 是C#的運算子語法


			//FamilySymbol val2 = (from FamilySymbol tag in (IEnumerable)new FilteredElementCollector(document).OfClass(typeof(FamilySymbol)).OfCategory((BuiltInCategory)(-2002000))
			//					 where Operators.CompareString(((Element)tag).Name, "RubberBand", TextCompare: false) == 0
			//					 select (tag)).First();
			FamilySymbol val2 = (from FamilySymbol tag in val
								 where Operators.CompareString(((Element)tag).Name, "RubberBand", TextCompare: false) == 0
								 select (tag)).First();
			//上面這是LINQ語法

			_added_element_ids.Clear();
			//每次要選取前都先清空list

			application2.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
			//revit的事件

			try
			{
				activeUIDocument.PromptForFamilyInstancePlacement(val2);
			}
			catch (Autodesk.Revit.Exceptions.OperationCanceledException val3)
			{
				ProjectData.SetProjectError((Exception)val3);
				
				Autodesk.Revit.Exceptions.OperationCanceledException val4 = val3;
				Debug.Print(((Exception)(object)val4).Message);
				ProjectData.ClearProjectError();
			}

			application2.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
			return Result.Succeeded;
		}
		private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
		{
			ICollection<ElementId> addedElementIds = e.GetAddedElementIds();
			int count = addedElementIds.Count;
			_added_element_ids.AddRange(addedElementIds);
			if (_added_element_ids.Count >= 1 && ComponentManager.ApplicationWindow != IntPtr.Zero)
			{
				//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//原程式有兩行，可以創建一次線段

				WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//如果修改成只有一行，可以創建多次線段

			}
		}

	}
}
