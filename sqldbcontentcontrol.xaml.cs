using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using Windows.Foundation;
//using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI;

namespace SqlDBObjects
{
	public enum DbObjectTypeKeys : int
	{
		Server,
		ServerSecurity,
		Database,
		Table,
		View,
		Programmability,
		Procedure,
		ExtendedProcedure,
		Function,
		CLRFunction,
		TableValuedFunction,
		CLRTableValuedFunction,
		Synonym,
		DatabaseSecurity,
		DBUser,
		DBRole,
		Schema,
		Login,
		ServerRole,
		Column,
		Parameter,
		ReturnValue,
		Default,
		Constraints,
		PrimaryKeyConstraint,
		UniqueConstraint,
		ForeignKeyConstraint,
		CheckConstraint,
		Indexes,
		PrimaryKeyIndex,
		UniqueConstIndex,
		ClusteredIndex,
		NonclusteredIndex,
		ClusteredColumnstoreIndex,
		NonclusteredColumnstoreIndex,
		PrimaryXMLIndex,
		SecondaryXMLIndex,
		SpatialIndex,
		Trigger,
		NotObject
	};
	public class DbObject : Windows.UI.Xaml.DependencyObject, System.ComponentModel.INotifyPropertyChanged, System.IEquatable<DbObject>,
		System.IComparable<DbObject>
	{
		public static Windows.UI.Xaml.DependencyProperty NameProperty = Windows.UI.Xaml.DependencyProperty.Register("Name", typeof(System.String), typeof(DbObject),
			new Windows.UI.Xaml.PropertyMetadata(null));
		private int propID = 0;
		private int propDatabaseID = 0;
		private int propParentObjectID = 0;
		private bool propIsSystemObject = false;
		private bool propIsFixedRole = false;
		private bool propIsSelected = false;
		private bool propIsExpanded = false;
		private bool propIsGroup = false;
		private bool propIsSubObjectsFilled = false;
		private DbObjectTypeKeys propTypeKey;
		private DbObject propParent = null;
		private System.Collections.ObjectModel.ObservableCollection<DbObject> propChildren = null;
		private Windows.UI.Xaml.Controls.TreeViewItem propTreeViewItem = null;
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged = delegate { };
		public System.String Name
		{
			get => (System.String)GetValue(NameProperty);
			set
			{
				if(System.String.Compare(value, (System.String)GetValue(NameProperty)) != 0)
				{
					SetValue(NameProperty, value);
					RaisePropertyChanged("Name");
				}
			}
		}
		public int ID
		{
			get => propID;
			set
			{
				if(value != propID)
				{
					propID = value;
					RaisePropertyChanged("ID");
				}
			}
		}
		public int DatabaseID
		{
			get => propDatabaseID;
			set
			{
				if(value != propDatabaseID)
				{
					propDatabaseID = value;
					RaisePropertyChanged("DatabaseID");
				}
			}
		}
		public int ParentObjectID
		{
			get => propParentObjectID;
			set
			{
				if(value != propParentObjectID)
				{
					propParentObjectID = value;
					RaisePropertyChanged("ParentObjectID");
				}
			}
		}
		public bool IsSystemObject
		{
			get => propIsSystemObject;
			set
			{
				if(value != propIsSystemObject)
				{
					propIsSystemObject = value;
					RaisePropertyChanged("IsSystemObject");
				}
			}
		}
		public bool IsFixedRole
		{
			get => propIsFixedRole;
			set
			{
				if(value != propIsFixedRole)
				{
					propIsFixedRole = value;
					RaisePropertyChanged("IsFixedRole");
				}
			}
		}
		public bool HasItems { get => Children != null && Children.Count > 0; }
		public bool IsSelected
		{
			get => propIsSelected;
			set
			{
				if(value != propIsSelected)
				{
					propIsSelected = value;
					RaisePropertyChanged("IsSelected");
				}
			}
		}
		public bool IsExpanded
		{
			get => propIsExpanded;
			set
			{
				if(value != propIsExpanded)
				{
					propIsExpanded = value;
					RaisePropertyChanged("IsExpanded");
					if(propIsExpanded && propParent != null && !propParent.IsExpanded)
					{
						propParent.IsExpanded = true;
					}
				}
			}
		}
		public bool IsGroup
		{
			get => propIsGroup;
			set
			{
				if(value != propIsGroup)
				{
					propIsGroup = value;
					RaisePropertyChanged("IsGroup");
					if(propIsGroup && propParent != null && !propParent.IsGroup)
					{
						propParent.IsGroup = true;
					}
				}
			}
		}
		public bool IsSubObjectsFilled
		{
			get => propIsSubObjectsFilled;
			set
			{
				if(value != propIsSubObjectsFilled)
				{
					propIsSubObjectsFilled = value;
					RaisePropertyChanged("IsSubObjectsFilled");
				}
			}
		}
		public DbObjectTypeKeys TypeKey
		{
			get => propTypeKey;
			set
			{
				if(value != propTypeKey)
				{
					propTypeKey = value;
					RaisePropertyChanged("TypeKey");
				}
			}
		}
		public DbObject Parent
		{
			get => propParent;
			set
			{
				if(value != propParent)
				{
					propParent = value;
					RaisePropertyChanged("Parent");
				}
			}
		}
		public System.Collections.ObjectModel.ObservableCollection<DbObject> Children
		{
			get => propChildren;
			set
			{
				if(value != propChildren)
				{
					propChildren = value;
					if(propChildren != null)
					{
						propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
					}
					RaisePropertyChanged("Parent");
				}
			}
		}
		public Windows.UI.Xaml.Controls.TreeViewItem TreeViewItem
		{
			get => propTreeViewItem;
			set
			{
				if(value != propTreeViewItem)
				{
					propTreeViewItem = value;
					RaisePropertyChanged("TreeViewItem");
					if(propParent != null && !propParent.IsExpanded)
					{
						propParent.IsExpanded = true;
					}
				}
			}
		}
		public DbObject()
		{
		}
		public DbObject(DbObject item)
		{
			if(item != null)
			{
				SetValue(NameProperty, item.Name);
				IsSelected = item.IsSelected;
				IsExpanded = item.IsExpanded;
				propIsGroup = item.IsGroup;
				propTypeKey = item.TypeKey;
				propParent = item.Parent;
				propChildren = item.Children;
				TreeViewItem = item.TreeViewItem;
				if(propChildren != null)
				{
					propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
				}
			}
			else
			{
				IsSelected = false;
				IsExpanded = false;
				propIsGroup = false;
				propParent = null;
				propChildren = null;
				TreeViewItem = null;
			}
		}
		public DbObject(DbObjectTypeKeys TKey)
		{
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = false;
			propTypeKey = TKey;
			propParent = null;
			if(propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(DbObjectTypeKeys TKey, bool fIsGroup)
		{
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propTypeKey = TKey;
			propParent = null;
			if(fIsGroup || propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(DbObjectTypeKeys TKey, DbObject curParent)
		{
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = false;
			propTypeKey = TKey;
			propParent = curParent;
			if(propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(DbObjectTypeKeys TKey, DbObject curParent, bool fIsGroup)
		{
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propTypeKey = TKey;
			propParent = curParent;
			if(fIsGroup || propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(DbObject curParent, bool fIsGroup)
		{
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propParent = curParent;
			propChildren = null;
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObject curParent)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = false;
			propParent = curParent;
			propChildren = null;
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObject curParent, bool fIsGroup)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propParent = curParent;
			if(fIsGroup)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObjectTypeKeys TKey)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = false;
			propTypeKey = TKey;
			propParent = null;
			if(propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObjectTypeKeys TKey, bool fIsGroup)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propTypeKey = TKey;
			propParent = null;
			if(fIsGroup || propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObjectTypeKeys TKey, DbObject curParent)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = false;
			propTypeKey = TKey;
			propParent = curParent;
			if(propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public DbObject(System.String NewName, DbObjectTypeKeys TKey, DbObject curParent, bool fIsGroup)
		{
			SetValue(NameProperty, NewName);
			IsSelected = false;
			IsExpanded = false;
			propIsGroup = fIsGroup;
			propTypeKey = TKey;
			propParent = curParent;
			if(fIsGroup || propTypeKey == DbObjectTypeKeys.Server)
			{
				propChildren = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				propChildren.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			}
			else
			{
				propChildren = null;
			}
			TreeViewItem = null;
		}
		public void RaisePropertyChanged(System.String strPropertyName)
		{
			PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(strPropertyName));
		}
		protected void OnCollectionChanged(System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RaisePropertyChanged("Children");
			if(Parent != null)
			{
				Parent.PropertyChanged(Parent, new System.ComponentModel.PropertyChangedEventArgs("Children"));
			}
			else
			{
				SqlDbContentControl contentControl = Window.Current.Content as SqlDbContentControl;
				contentControl.RaisePropertyChanged("DbObjects");
			}
		}
		public override System.String ToString()
		{
			System.String strResult = null;
			if(Name != null)
			{
				strResult = Name;
			}
			else if((int)propTypeKey != 0)
			{
				strResult = System.Enum.GetName(typeof(DbObjectTypeKeys), propTypeKey);
			}
			return strResult;
		}
		public bool Equals(DbObject itemOther)
		{
			return itemOther != null && Name.CompareTo(itemOther.Name) == 0;
		}
		public int CompareTo(DbObject itemOther)
		{
			return itemOther != null ? Name.CompareTo(itemOther.Name) : 1;
		}
		public System.String GetResourceKeyForObjectTypeImage()
		{
			System.String strResourceKey = null;
			if(IsGroup)
			{
				strResourceKey = "IMG_GROUP";
			}
			else
			{
				switch(TypeKey)
				{
				case DbObjectTypeKeys.Server:
					strResourceKey = "IMG_SERVER";
					break;
				case DbObjectTypeKeys.Database:
					strResourceKey = "IMG_DATABASE";
					break;
				case DbObjectTypeKeys.Table:
					strResourceKey = "IMG_TABLE";
					break;
				case DbObjectTypeKeys.View:
					strResourceKey = "IMG_VIEW";
					break;
				case DbObjectTypeKeys.Procedure:
					strResourceKey = "IMG_PROCEDURE";
					break;
				case DbObjectTypeKeys.ExtendedProcedure:
					strResourceKey = "IMG_PROCEDURE";
					break;
				case DbObjectTypeKeys.Function:
					strResourceKey = "IMG_SCALAR_FUNCTION";
					break;
				case DbObjectTypeKeys.CLRFunction:
					strResourceKey = "IMG_SCALAR_FUNCTION";
					break;
				case DbObjectTypeKeys.TableValuedFunction:
					strResourceKey = "IMG_TABLE_FUNCTION";
					break;
				case DbObjectTypeKeys.CLRTableValuedFunction:
					strResourceKey = "IMG_TABLE_FUNCTION";
					break;
				case DbObjectTypeKeys.Synonym:
					strResourceKey = "IMG_SYNONYM";
					break;
				case DbObjectTypeKeys.DBUser:
					strResourceKey = "IMG_DBUSER";
					break;
				case DbObjectTypeKeys.DBRole:
					strResourceKey = "IMG_DBROLE";
					break;
				case DbObjectTypeKeys.Schema:
					strResourceKey = "IMG_SCHEMA";
					break;
				case DbObjectTypeKeys.Login:
					strResourceKey = "IMG_LOGIN";
					break;
				case DbObjectTypeKeys.ServerRole:
					strResourceKey = "IMG_SERVER_ROLE";
					break;
				case DbObjectTypeKeys.Column:
					strResourceKey = "IMG_COLUMN";
					break;
				case DbObjectTypeKeys.Parameter:
					strResourceKey = "IMG_PARAMETER";
					break;
				case DbObjectTypeKeys.ReturnValue:
					strResourceKey = "IMG_RETVALUE";
					break;
				case DbObjectTypeKeys.Default:
					strResourceKey = "IMG_";
					break;
				case DbObjectTypeKeys.PrimaryKeyConstraint:
					strResourceKey = "IMG_PRIMARY_KEY";
					break;
				case DbObjectTypeKeys.UniqueConstraint:
					strResourceKey = "IMG_UNIQUE";
					break;
				case DbObjectTypeKeys.ForeignKeyConstraint:
					strResourceKey = "IMG_FOREIGN_KEY";
					break;
				case DbObjectTypeKeys.CheckConstraint:
					strResourceKey = "IMG_CHECK";
					break;
				case DbObjectTypeKeys.PrimaryKeyIndex:
					strResourceKey = "IMG_PRIMARY_KEY";
					break;
				case DbObjectTypeKeys.UniqueConstIndex:
					strResourceKey = "IMG_UNIQUE";
					break;
				case DbObjectTypeKeys.ClusteredIndex:
					strResourceKey = "IMG_CLUST_INDEX";
					break;
				case DbObjectTypeKeys.NonclusteredIndex:
					strResourceKey = "IMG_INDEX";
					break;
				case DbObjectTypeKeys.ClusteredColumnstoreIndex:
					strResourceKey = "IMG_COLUMNSTORE_INDEX";
					break;
				case DbObjectTypeKeys.NonclusteredColumnstoreIndex:
					strResourceKey = "IMG_COLUMNSTORE_INDEX";
					break;
				case DbObjectTypeKeys.PrimaryXMLIndex:
					strResourceKey = "IMG_INDEX";
					break;
				case DbObjectTypeKeys.SecondaryXMLIndex:
					strResourceKey = "IMG_INDEX";
					break;
				case DbObjectTypeKeys.SpatialIndex:
					strResourceKey = "IMG_INDEX";
					break;
				case DbObjectTypeKeys.Trigger:
					strResourceKey = "IMG_TRIGGER";
					break;
				}
			}
			return strResourceKey;
		}
		public Windows.UI.Xaml.Media.ImageSource GetImageSourceForObjectTypeImage()
		{
			Windows.UI.Xaml.Media.ImageSource imageSource = null;
			System.String strResourceKey = GetResourceKeyForObjectTypeImage();
			if(!System.String.IsNullOrEmpty(strResourceKey))
			{
				SqlDbContentControl contentControl = Window.Current.Content as SqlDbContentControl;
				if(contentControl.Resources != null && contentControl.Resources.ContainsKey(strResourceKey))
				{
					imageSource = contentControl.Resources[strResourceKey] as Windows.UI.Xaml.Media.ImageSource;
				}
				else if(Application.Current.Resources != null && Application.Current.Resources.ContainsKey(strResourceKey))
				{
					imageSource = Application.Current.Resources[strResourceKey] as Windows.UI.Xaml.Media.ImageSource;
				}
			}
			return imageSource;
		}
	};
	public sealed partial class SqlDbContentControl : ContentControl, INotifyPropertyChanged
	{
		public static Windows.UI.Xaml.DependencyProperty ItemsProperty = Windows.UI.Xaml.DependencyProperty.Register("Items",
			typeof(System.Collections.ObjectModel.ObservableCollection<DbObject>), typeof(SqlDbContentControl),
			new Windows.UI.Xaml.PropertyMetadata(new System.Collections.ObjectModel.ObservableCollection<DbObject>()));
		public static Windows.UI.Xaml.DependencyProperty SelectedItemProperty = Windows.UI.Xaml.DependencyProperty.Register("SelectedItem", typeof(DbObject),
			typeof(SqlDbContentControl), new Windows.UI.Xaml.PropertyMetadata(null));
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		public static System.String DefaultConnectionString { get; set; } = "Server=STANISLAVWS; Database=master; Trusted_Connection=yes;";
		public static System.Data.SqlClient.SqlConnection SqlConnection { get; set; }
		public System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.DependencyObject> DialogControls { get; set; }
		public bool IsClosed { get; set; }
		public Windows.UI.Xaml.Style TreeViewItemStyle { get; set; }
		public Windows.UI.Xaml.Controls.ControlTemplate TreeViewItemTemplate { get; set; }
		public Windows.UI.Xaml.DataTemplate TreeViewItemDataTemplate { get; set; }
		public static System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.Controls.MenuFlyoutItemBase> DbObjectMenuItems { get; set; } =
			new System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.Controls.MenuFlyoutItemBase>();
		public System.Collections.ObjectModel.ObservableCollection<DbObject> Items
		{
			get => (System.Collections.ObjectModel.ObservableCollection<DbObject>)GetValue(ItemsProperty);
			set
			{
				if(value != (System.Collections.ObjectModel.ObservableCollection<DbObject>)GetValue(ItemsProperty))
				{
					SetValue(ItemsProperty, value);
					if(value != null)
					{
						Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
					}
					RaisePropertyChanged("Items");
				}
			}
		}
		public DbObject SelectedItem
		{
			get => (DbObject)GetValue(SelectedItemProperty);
			set
			{
				if(value != (DbObject)GetValue(SelectedItemProperty))
				{
					SetValue(SelectedItemProperty, value);
					DbObject item = (DbObject)value;
					if(item != null && !item.IsSelected)
					{
						item.IsSelected = true;
					}
					RaisePropertyChanged("SelectedItem");
				}
			}
		}
		public DbObject ContextMenuTargetItem { get; set; } = null;
		public SqlDbContentControl()
        {
			DataContext = this;
			Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnCollectionChanged);
			this.InitializeComponent();
        }
		protected void OnLoaded(System.Object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			DialogControls = new System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.DependencyObject>();
			App.FillDialogControls(DialogControls, Content);
			if(Resources == null || DialogControls.Count == 0
				|| !Resources.ContainsKey("DBOBJECT_TEMPLATE")
			 	|| !DialogControls.ContainsKey("DBObjectTree")
			 	|| !DialogControls.ContainsKey("ResultDataGrid") || !DialogControls.ContainsKey("RowsetDataGrid")
				|| !DialogControls.ContainsKey("CommandBorder")
				|| !DialogControls.ContainsKey("CommandTextBox"))
			{
				App.ShowMessage("XAML is not loaded correctly.");
				Application.Current.Exit();
			}
			TreeViewItemDataTemplate = (Windows.UI.Xaml.DataTemplate)Resources["DBOBJECT_TEMPLATE"];
			Windows.UI.Xaml.Controls.TreeViewItem tvitem = TreeViewItemDataTemplate.LoadContent() as Windows.UI.Xaml.Controls.TreeViewItem;
			if(tvitem != null)
			{
				Windows.UI.Xaml.Controls.MenuFlyout contentMenu = tvitem.ContextFlyout as Windows.UI.Xaml.Controls.MenuFlyout;
				if(contentMenu != null)
				{
					AddMenuFlyoutItemsMapping(contentMenu, DbObjectMenuItems);
				}
			}
			FillDBObjects();
		}
		public void RaisePropertyChanged(System.String strPropertyName)
		{
			PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(strPropertyName));
		}
		protected void OnCollectionChanged(System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				Windows.UI.Xaml.Controls.TreeView treeview = (Windows.UI.Xaml.Controls.TreeView)DialogControls["DBObjectTree"];
				treeview.UpdateLayout();
			}
			RaisePropertyChanged("Items");
		}
		protected void CommandCanExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.CanExecuteRequestedEventArgs e)
		{
			e.CanExecute = true;	//(Windows.UI.Xaml.Controls.Control)e.Parameter != null;
		}
		protected async void CommandOKExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			Windows.UI.Xaml.Controls.ContentDialogResult dialogResult = await App.ShowContentDialogAsync("Apply changes and close?", "Yes", "No");
			if(dialogResult == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
			{
				Windows.UI.Xaml.Application.Current.Exit();
			}
		}
		protected async void CommandCancelExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			Windows.UI.Xaml.Controls.ContentDialogResult dialogResult = await App.ShowContentDialogAsync("Close without saving changes?", "Yes", "No");
			if(dialogResult == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
			{
				Windows.UI.Xaml.Application.Current.Exit();
			}
		}
		protected void CommandCreateScriptExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			App.ShowMessage("Command CreateScript execute");
		}
		protected void CommandOpenScriptExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandSaveScriptExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandSaveAllExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandExecScriptExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandShowResultExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandCloseRowsetExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridRowset = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["RowsetDataGrid"];
			Windows.UI.Xaml.Controls.Border borderCommand = (Windows.UI.Xaml.Controls.Border)DialogControls["CommandBorder"];
			dataGridRowset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			borderCommand.Visibility = Windows.UI.Xaml.Visibility.Visible;
		}
		protected void CommandUndoExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandRedoExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandCutExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandCopyExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandPasteExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandFindExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		protected void CommandObjectCreateExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
		}
		void CommandObjectOpenExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
				DbObject itemDatabase = null;
				switch(ContextMenuTargetItem.TypeKey)
				{
				case DbObjectTypeKeys.Database:
					EstablishConnection(ContextMenuTargetItem.Name);
					break;
				case DbObjectTypeKeys.Table:
				case DbObjectTypeKeys.View:
					if(ContextMenuTargetItem.Parent != null && (itemDatabase = ContextMenuTargetItem.Parent.Parent) != null && EstablishConnection())
					{
						Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridRowset = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["RowsetDataGrid"];
						if(FillDataGridFromSqlQuery(dataGridRowset, "SELECT * FROM " + itemDatabase.Name + "." + ContextMenuTargetItem.Name))
						{
							ShowRowsetDataGrid();
						}
					}
					break;
				}
			}
		}
		protected void CommandObjectExecExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null && !System.String.IsNullOrEmpty(ContextMenuTargetItem.Name))
			{
				Windows.UI.Xaml.Controls.TextBox textboxCommand = (Windows.UI.Xaml.Controls.TextBox)DialogControls["CommandTextBox"];
				textboxCommand.Text = "";
				if(ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.Procedure || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.ExtendedProcedure
					|| ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.Function || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.CLRFunction
					|| ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.TableValuedFunction || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.CLRTableValuedFunction)
				{
					if(EstablishConnection())
					{
						Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridResult = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["ResultDataGrid"];
						if(FillDataGridFromSqlQuery(dataGridResult, ContextMenuTargetItem.Name, System.Data.CommandType.StoredProcedure))
						{
							ShowResultDataGrid();
						}
					}
				}
			}
		}
		protected void CommandObjectScriptCreateExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptAlterExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptDropExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptSelectExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptInsertExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptUpdateExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptDeleteExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectScriptExecExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectCheckExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectNoCheckExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectEnableExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectDisableExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectRebuildExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectReorganizeExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectModifyExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectRenameExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectDropExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
			}
		}
		protected void CommandObjectPropertiesExecute(Windows.UI.Xaml.Input.XamlUICommand command, Windows.UI.Xaml.Input.ExecuteRequestedEventArgs e)
		{
			if(ContextMenuTargetItem != null)
			{
				//DbObjectDialog.Item = curitem;
				//DbObjectDialog.Show();
			}
		}
		protected void OnTreeViewItemInvoked(Microsoft.UI.Xaml.Controls.TreeView treeview, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs e)
		{
			SelectedItem = e.InvokedItem as DbObject;
		}
		protected void OnTreeViewExpanding(Microsoft.UI.Xaml.Controls.TreeView treeview, Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs e)
		{
			DbObject item = null;
			if(e.Item != null && (item = e.Item as DbObject) != null)
			{
				if(!item.IsGroup && (item.TypeKey == DbObjectTypeKeys.Database
					|| item.TypeKey == DbObjectTypeKeys.Table || item.TypeKey == DbObjectTypeKeys.View
					|| item.TypeKey == DbObjectTypeKeys.Procedure || item.TypeKey == DbObjectTypeKeys.Function
					|| item.TypeKey == DbObjectTypeKeys.CLRFunction || item.TypeKey == DbObjectTypeKeys.TableValuedFunction
					|| item.TypeKey == DbObjectTypeKeys.CLRTableValuedFunction)
					&& !item.IsSubObjectsFilled && item.Children != null && item.Children.Count > 0)
				{
					if(item.TypeKey == DbObjectTypeKeys.Database)
					{
						DbObject itemTables = item.Children[0];
						DbObject itemViews = item.Children[1];
						DbObject itemProgrammability = item.Children[2];
						DbObject itemProcedures = itemProgrammability.Children[0];
						DbObject itemXProcedures = itemProgrammability.Children[1];
						DbObject itemFunctions = itemProgrammability.Children[2];
						DbObject itemTFunctions = itemProgrammability.Children[3];
						AppendDBObjectsFromSqlQuery(itemTables, DbObjectTypeKeys.Table, GetQueryForSelectDatabaseChildren(item, "U"));
						AppendDBObjectsFromSqlQuery(itemViews, DbObjectTypeKeys.View, GetQueryForSelectDatabaseChildren(item, "V"));
						AppendDBObjectsFromSqlQuery(itemProcedures, DbObjectTypeKeys.Procedure, GetQueryForSelectDatabaseChildren(item, "P"));
						AppendDBObjectsFromSqlQuery(itemXProcedures, DbObjectTypeKeys.ExtendedProcedure, GetQueryForSelectDatabaseChildren(item, "X"));
						AppendDBObjectsFromSqlQuery(itemFunctions, DbObjectTypeKeys.Function, GetQueryForSelectDatabaseChildren(item, "FN"));
						AppendDBObjectsFromSqlQuery(itemFunctions, DbObjectTypeKeys.CLRFunction, GetQueryForSelectDatabaseChildren(item, "FS"));
						AppendDBObjectsFromSqlQuery(itemTFunctions, DbObjectTypeKeys.TableValuedFunction, GetQueryForSelectDatabaseChildren(item, "TF"));
						AppendDBObjectsFromSqlQuery(itemTFunctions, DbObjectTypeKeys.CLRTableValuedFunction, GetQueryForSelectDatabaseChildren(item, "FT"));
						FillDatabasePrincipals(item.Name, item.Children[3]);
						foreach(DbObject curitem in itemTables.Children)
						{
							curitem.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
							curitem.Children.Add(new DbObject("Columns", DbObjectTypeKeys.Column, curitem, true));
							curitem.Children.Add(new DbObject("Keys", DbObjectTypeKeys.Constraints, curitem, true));
							curitem.Children.Add(new DbObject("Checks", DbObjectTypeKeys.CheckConstraint, curitem, true));
							curitem.Children.Add(new DbObject("Indexes", DbObjectTypeKeys.Indexes, curitem, true));
							curitem.Children.Add(new DbObject("Triggers", DbObjectTypeKeys.Trigger, curitem, true));
						}
						foreach(DbObject curitem in itemViews.Children)
						{
							curitem.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
							curitem.Children.Add(new DbObject("Columns", DbObjectTypeKeys.Column, curitem, true));
							curitem.Children.Add(new DbObject("Indexes", DbObjectTypeKeys.Indexes, curitem, true));
							curitem.Children.Add(new DbObject("Triggers", DbObjectTypeKeys.Trigger, curitem, true));
						}
						foreach(DbObject curitem in itemProcedures.Children)
						{
							curitem.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
							curitem.Children.Add(new DbObject("Parameters", DbObjectTypeKeys.Parameter, curitem, true));
						}
						foreach(DbObject curitem in itemFunctions.Children)
						{
							curitem.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
							curitem.Children.Add(new DbObject("Parameters", DbObjectTypeKeys.Parameter, curitem, true));
						}
						foreach(DbObject curitem in itemTFunctions.Children)
						{
							curitem.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
							curitem.Children.Add(new DbObject("Parameters", DbObjectTypeKeys.Parameter, curitem, true));
						}
					}
					else if(item.TypeKey == DbObjectTypeKeys.Table || item.TypeKey == DbObjectTypeKeys.View)
					{
						int idx = 0;
						AppendDBObjectsFromSqlQuery(item.Children[idx++], DbObjectTypeKeys.Column, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.Column));
						if(item.TypeKey == DbObjectTypeKeys.Table)
						{
							AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.PrimaryKeyConstraint, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.PrimaryKeyConstraint));
							AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.UniqueConstraint, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.UniqueConstraint));
							AppendDBObjectsFromSqlQuery(item.Children[idx++], DbObjectTypeKeys.ForeignKeyConstraint, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.ForeignKeyConstraint));
							AppendDBObjectsFromSqlQuery(item.Children[idx++], DbObjectTypeKeys.CheckConstraint, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.CheckConstraint));
						}
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.PrimaryKeyIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.PrimaryKeyIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.UniqueConstIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.UniqueConstIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.ClusteredIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.ClusteredIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.NonclusteredIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.NonclusteredIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.ClusteredColumnstoreIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.ClusteredColumnstoreIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.NonclusteredColumnstoreIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.NonclusteredColumnstoreIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.PrimaryXMLIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.PrimaryXMLIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.SecondaryXMLIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.SecondaryXMLIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx++], DbObjectTypeKeys.SpatialIndex, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.SpatialIndex));
						AppendDBObjectsFromSqlQuery(item.Children[idx], DbObjectTypeKeys.Trigger, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.Trigger));
					}
					else if(item.TypeKey == DbObjectTypeKeys.Procedure || item.TypeKey == DbObjectTypeKeys.Function
						|| item.TypeKey == DbObjectTypeKeys.CLRFunction || item.TypeKey == DbObjectTypeKeys.TableValuedFunction
						|| item.TypeKey == DbObjectTypeKeys.CLRTableValuedFunction)
					{
						AppendDBObjectsFromSqlQuery(item.Children[0], DbObjectTypeKeys.Parameter, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.Parameter));
						if(item.TypeKey == DbObjectTypeKeys.Procedure)
						{
							if(item.Children[0].Children != null && item.Children[0].Children.Count > 0)
							{
								item.Children[0].Children.Add(new DbObject("Returns int", DbObjectTypeKeys.ReturnValue, item.Children[0]));
							}
						}
						else if(item.TypeKey == DbObjectTypeKeys.Function || item.TypeKey == DbObjectTypeKeys.CLRFunction)
						{
							AppendDBObjectsFromSqlQuery(item.Children[0], DbObjectTypeKeys.ReturnValue, GetQueryForSelectDBObjectChildren(item, DbObjectTypeKeys.ReturnValue));
						}
					}
					item.IsSubObjectsFilled = true;
				}
			}
		}
		protected void OnDbObjectContextMenuOpened(System.Object sender, System.Object parameter)
		{
			Windows.UI.Xaml.Controls.MenuFlyout contextMenu = (Windows.UI.Xaml.Controls.MenuFlyout)sender;
			ContextMenuTargetItem = null;
			if(contextMenu != null)
			{
				foreach(Windows.UI.Xaml.Controls.MenuFlyoutItemBase menuitem in DbObjectMenuItems.Values)
				{
					menuitem.IsEnabled = false;
				}
				if(contextMenu.Target != null && contextMenu.Target is Windows.UI.Xaml.Controls.TreeViewItem)
				{
					Windows.UI.Xaml.Controls.TreeView treeview = (Windows.UI.Xaml.Controls.TreeView)DialogControls["DBObjectTree"];
					Windows.UI.Xaml.Controls.TreeViewItem tvitem = (Windows.UI.Xaml.Controls.TreeViewItem)contextMenu.Target;
					if(treeview != null && tvitem != null && (ContextMenuTargetItem = treeview.ItemFromContainer(tvitem) as DbObject) != null)
					{
						switch(ContextMenuTargetItem.TypeKey)
						{
						case DbObjectTypeKeys.Table:
						case DbObjectTypeKeys.View:
						case DbObjectTypeKeys.Procedure:
						case DbObjectTypeKeys.Function:
						case DbObjectTypeKeys.CLRFunction:
						case DbObjectTypeKeys.TableValuedFunction:
						case DbObjectTypeKeys.CLRTableValuedFunction:
							if(ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.Table || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.View)
							{
								DbObjectMenuItems["Open"].IsEnabled = true;
								DbObjectMenuItems["ScriptSelect"].IsEnabled = true;
								DbObjectMenuItems["ScriptInsert"].IsEnabled = true;
								DbObjectMenuItems["ScriptUpdate"].IsEnabled = true;
								DbObjectMenuItems["ScriptDelete"].IsEnabled = true;
							}
							else if(ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.Function || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.CLRFunction
								|| ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.TableValuedFunction || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.CLRTableValuedFunction)
							{
								DbObjectMenuItems["ScriptSelect"].IsEnabled = true;
							}
							else
							{
								DbObjectMenuItems["Execute"].IsEnabled = true;
								DbObjectMenuItems["ScriptExecute"].IsEnabled = true;
							}
							goto case DbObjectTypeKeys.Trigger;
						case DbObjectTypeKeys.Trigger:
						case DbObjectTypeKeys.Column:
						case DbObjectTypeKeys.PrimaryKeyConstraint:
						case DbObjectTypeKeys.UniqueConstraint:
						case DbObjectTypeKeys.ForeignKeyConstraint:
						case DbObjectTypeKeys.CheckConstraint:
						case DbObjectTypeKeys.PrimaryKeyIndex:
						case DbObjectTypeKeys.UniqueConstIndex:
						case DbObjectTypeKeys.ClusteredIndex:
						case DbObjectTypeKeys.NonclusteredIndex:
						case DbObjectTypeKeys.ClusteredColumnstoreIndex:
						case DbObjectTypeKeys.NonclusteredColumnstoreIndex:
						case DbObjectTypeKeys.PrimaryXMLIndex:
						case DbObjectTypeKeys.SecondaryXMLIndex:
						case DbObjectTypeKeys.SpatialIndex:
						case DbObjectTypeKeys.DBUser:
						case DbObjectTypeKeys.DBRole:
						case DbObjectTypeKeys.Login:
						case DbObjectTypeKeys.ServerRole:
							if(ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.PrimaryKeyConstraint || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.UniqueConstraint
								|| ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.ForeignKeyConstraint || ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.CheckConstraint)
							{
								DbObjectMenuItems["Check"].IsEnabled = true;
								DbObjectMenuItems["NoCheck"].IsEnabled = true;
							}
							else if(ContextMenuTargetItem.TypeKey == DbObjectTypeKeys.Trigger)
							{
								DbObjectMenuItems["Enable"].IsEnabled = true;
								DbObjectMenuItems["Disable"].IsEnabled = true;
							}
							else
							{
								if(ContextMenuTargetItem.Parent != null && ContextMenuTargetItem.Parent.TypeKey == DbObjectTypeKeys.Indexes)
								{
									DbObjectMenuItems["Rebuild"].IsEnabled = true;
									DbObjectMenuItems["Reorganize"].IsEnabled = ContextMenuTargetItem.TypeKey != DbObjectTypeKeys.NonclusteredColumnstoreIndex;
									DbObjectMenuItems["Disable"].IsEnabled = true;
								}
								DbObjectMenuItems["Properties"].IsEnabled = true;
							}
							if(ContextMenuTargetItem.TypeKey != DbObjectTypeKeys.PrimaryKeyConstraint)
							{
								DbObjectMenuItems["Create"].IsEnabled = true;
							}
							DbObjectMenuItems["Modify"].IsEnabled = true;
							DbObjectMenuItems["Rename"].IsEnabled = true;
							DbObjectMenuItems["Drop"].IsEnabled = true;
							DbObjectMenuItems["ScriptCreate"].IsEnabled = true;
							DbObjectMenuItems["ScriptDrop"].IsEnabled = true;
							DbObjectMenuItems["ScriptMenu"].IsEnabled = true;
							break;
						case DbObjectTypeKeys.Server:
						case DbObjectTypeKeys.Database:
							DbObjectMenuItems["CreateScript"].IsEnabled = true;
							DbObjectMenuItems["Properties"].IsEnabled = true;
							break;
						case DbObjectTypeKeys.Parameter:
						case DbObjectTypeKeys.ReturnValue:
							break;
						}
					}
				}
			}
		}
		public bool EstablishConnection()
		{
			return EstablishConnection(null);
		}
		public bool EstablishConnection(System.String strDBName)
		{
			System.String strConnectionString = SqlConnection != null ? SqlConnection.ConnectionString : DefaultConnectionString;
			if(!System.String.IsNullOrEmpty(strDBName) && (SqlConnection == null || System.String.Compare(SqlConnection.Database, strDBName) != 0))
			{
				int nIndex1 = strConnectionString.IndexOf("Database=");
				int nLength = 9;
				if(nIndex1 > 0)
				{
					int nIndex2 = strConnectionString.IndexOf(";", nIndex1 + nLength);
					if(nIndex2 > 0)
					{
						strConnectionString = strConnectionString.Substring(0, nIndex1 + nLength) + strDBName + strConnectionString.Substring(nIndex2);
					}
					else
					{
						strConnectionString = strConnectionString.Substring(0, nIndex1 + nLength) + strDBName + ";";
					}
				}
				else
				{
					strConnectionString += ";Database=" + strDBName + ";";
				}
			}
			try
			{
				if(SqlConnection == null)
				{
					SqlConnection = new System.Data.SqlClient.SqlConnection(strConnectionString);
				}
				if(SqlConnection.State != System.Data.ConnectionState.Open || System.String.Compare(SqlConnection.ConnectionString, strConnectionString) != 0)
				{
					if(SqlConnection.State != System.Data.ConnectionState.Closed)
					{
						SqlConnection.Close();
					}
					SqlConnection.ConnectionString = strConnectionString;
					SqlConnection.Open();
				}
			}
			catch(System.Exception e)
			{
				App.ShowErrorMessage(e);
				SqlConnection = null;
				throw e;
			}
			return SqlConnection != null && SqlConnection.State == System.Data.ConnectionState.Open;
		}
		public void FillDBObjects()
		{
			if(EstablishConnection())
			{
				Items.Add(new DbObject(SqlConnection.DataSource, DbObjectTypeKeys.Server));
				Items[0].Children.Add(new DbObject("Databases", DbObjectTypeKeys.Database, Items[0], true));
				Items[0].Children.Add(new DbObject("Security", DbObjectTypeKeys.ServerSecurity, Items[0], true));
				FillDatabases(Items[0].Children[0]);
				FillServerPrincipals(Items[0].Children[1]);
			}
		}
		public void FillDatabases(DbObject curParent)
		{
			AppendDBObjectsFromSqlQuery(curParent, DbObjectTypeKeys.Database, "SELECT [database_id], [name] FROM sys.databases");
			foreach(DbObject curDatabase in curParent.Children)
			{
				FillDatabaseChildren(curDatabase);
			}
		}
		public void FillServerPrincipals(DbObject curParent)
		{
			if(curParent != null && curParent.Children != null)
			{
				curParent.Children.Add(new DbObject("Logins", DbObjectTypeKeys.Login, curParent, true));
				curParent.Children.Add(new DbObject("Server rols", DbObjectTypeKeys.Login, curParent, true));
				AppendDBObjectsFromSqlQuery(curParent.Children[0], DbObjectTypeKeys.Login, "SELECT [principal_id] [id], [name] FROM sys.server_principals WHERE [type] IN ('S', 'U', 'C', 'K')");
				AppendDBObjectsFromSqlQuery(curParent.Children[1], DbObjectTypeKeys.ServerRole, "SELECT [principal_id] [id], [name], [is_fixed_role] FROM sys.server_principals WHERE [type] = 'R'");
			}
		}
		public void FillDatabasePrincipals(System.String strDBName, DbObject curParent)
		{
			if(!System.String.IsNullOrEmpty(strDBName) && curParent != null && curParent.Children != null)
			{
				curParent.Children.Add(new DbObject("Users", DbObjectTypeKeys.Login, curParent, true));
				curParent.Children.Add(new DbObject("Rols", DbObjectTypeKeys.Login, curParent, true));
				curParent.Children.Add(new DbObject("Schemas", DbObjectTypeKeys.Schema, curParent, true));
				AppendDBObjectsFromSqlQuery(curParent.Children[0], DbObjectTypeKeys.Login,
					"USE " + strDBName + "\nSELECT [principal_id] [id], [name] FROM sys.database_principals WHERE [type] IN ('S', 'U', 'C', 'K')");
				AppendDBObjectsFromSqlQuery(curParent.Children[1], DbObjectTypeKeys.ServerRole,
					"USE " + strDBName + "\nSELECT [principal_id] [id], [name], [is_fixed_role] FROM sys.database_principals WHERE [type] = 'R'");
				AppendDBObjectsFromSqlQuery(curParent.Children[1], DbObjectTypeKeys.ServerRole,
					"USE " + strDBName + "\nSELECT principal_id [parent_object_id], schema_id [id], [name] FROM sys.schemas");
			}
		}
		public void FillDatabaseChildren(DbObject curParent)
		{
			if(curParent != null && !System.String.IsNullOrEmpty(curParent.Name))
			{
				if(curParent.Children == null)
				{
					curParent.Children = new System.Collections.ObjectModel.ObservableCollection<DbObject>();
				}
				curParent.Children.Add(new DbObject("Tables", DbObjectTypeKeys.Table, curParent, true));
				curParent.Children.Add(new DbObject("Views", DbObjectTypeKeys.View, curParent, true));
				curParent.Children.Add(new DbObject("Programmability", DbObjectTypeKeys.Programmability, curParent, true));
				curParent.Children.Add(new DbObject("Security", DbObjectTypeKeys.DatabaseSecurity, curParent, true));
				DbObject itemProgrammability = curParent.Children[2];
				itemProgrammability.Children.Add(new DbObject("Procedures", DbObjectTypeKeys.Procedure, itemProgrammability, true));
				itemProgrammability.Children.Add(new DbObject("Extended procedures", DbObjectTypeKeys.ExtendedProcedure, itemProgrammability, true));
				itemProgrammability.Children.Add(new DbObject("Scalar functions", DbObjectTypeKeys.Function, itemProgrammability, true));
				itemProgrammability.Children.Add(new DbObject("Table valued functions", DbObjectTypeKeys.TableValuedFunction, itemProgrammability, true));
			}
		}
		public System.String GetQueryForSelectDatabaseChildren(DbObject database, System.String strObjectType)
		{
			strObjectType = strObjectType.Trim();
			strObjectType = strObjectType.Replace(" ", "");
			strObjectType = strObjectType.Replace(",", "', '");
			System.String strCommandText =
				"USE " + database.Name + @" 
				SELECT
					" + database.DatabaseID.ToString() + @"[database_id],
					[object_id] [id],
					SCHEMA_NAME([schema_id]) +'.' + [name][name],
					CONVERT(bit,
						CASE
							WHEN SCHEMA_NAME([schema_id]) = 'sys'
								THEN 1
							ELSE 0
						END) is_system_object
				FROM
					" + database.Name + @".sys.all_objects
				WHERE[type] IN('" + strObjectType + "')";
			return strCommandText;
		}
		public System.String GetQueryForSelectDBObjectChildren(DbObject item, DbObjectTypeKeys tkeySubObject)
		{
			System.String strCommandText = null;
			System.String strDBName = null;
			System.String strDB_ID = null;
			DbObject curParent = item;
			while(strDBName == null && (curParent = curParent.Parent) != null)
			{
				if(curParent.TypeKey == DbObjectTypeKeys.Database)
				{
					strDBName = curParent.Name;
					strDB_ID = curParent.DatabaseID > 0 ? curParent.DatabaseID.ToString() : "";
				}
			}
			if(!System.String.IsNullOrEmpty(strDBName) && !System.String.IsNullOrEmpty(strDB_ID))
			{
				System.String strObjectIDColumnName = "[object_id]";
				System.String strSubObjectTableName = "";
				System.String strSubObjectIDColumnName = "object_id";
				System.String strSubObjectNameColumnName = "[name]";
				System.String strSubObjectCondition = "\n\tAND [name] IS NOT NULL";
				System.String strSubObjectType = "";
				switch(tkeySubObject)
				{
				case DbObjectTypeKeys.Column:
					strSubObjectTableName = "sys.all_columns";
					strSubObjectIDColumnName = "column_id";
					break;
				case DbObjectTypeKeys.Parameter:
					strSubObjectTableName = "sys.all_parameters";
					strSubObjectIDColumnName = "parameter_id";
					break;
				case DbObjectTypeKeys.ReturnValue:
					strSubObjectTableName = "sys.all_parameters";
					strSubObjectIDColumnName = "parameter_id";
					strSubObjectNameColumnName = "CONVERT(sysname, 'Returns ' + TYPE_NAME(user_type_id))";
					strSubObjectCondition = "\n\tAND parameter_id = 0";
					break;
				case DbObjectTypeKeys.PrimaryKeyConstraint:
				case DbObjectTypeKeys.UniqueConstraint:
					strSubObjectTableName = "sys.key_constraints";
					strObjectIDColumnName = "parent_object_id";
					strSubObjectCondition += "\n\tAND [type] = " + (tkeySubObject == DbObjectTypeKeys.PrimaryKeyConstraint ? "'PK'" : "'UQ'");
					break;
				case DbObjectTypeKeys.ForeignKeyConstraint:
					strSubObjectTableName = "sys.foreign_keys";
					strObjectIDColumnName = "parent_object_id";
					break;
				case DbObjectTypeKeys.CheckConstraint:
					strSubObjectTableName = "sys.check_constraints";
					strObjectIDColumnName = "parent_object_id";
					break;
				case DbObjectTypeKeys.PrimaryKeyIndex:
				case DbObjectTypeKeys.UniqueConstIndex:
					strSubObjectTableName = "sys.indexes";
					strSubObjectIDColumnName = "index_id";
					strSubObjectCondition += "\n\tAND "
						+ (tkeySubObject == DbObjectTypeKeys.PrimaryKeyIndex ? "ISNULL(is_primary_key, 0) = 1" : "ISNULL(is_unique_constraint, 0) = 1");
					break;
				case DbObjectTypeKeys.ClusteredIndex:
				case DbObjectTypeKeys.NonclusteredIndex:
				case DbObjectTypeKeys.ClusteredColumnstoreIndex:
				case DbObjectTypeKeys.NonclusteredColumnstoreIndex:
				case DbObjectTypeKeys.PrimaryXMLIndex:
				case DbObjectTypeKeys.SecondaryXMLIndex:
				case DbObjectTypeKeys.SpatialIndex:
					switch(tkeySubObject)
					{
					case DbObjectTypeKeys.ClusteredIndex:
						strSubObjectType = "1";
						break;
					case DbObjectTypeKeys.NonclusteredIndex:
						strSubObjectType = "2";
						break;
					case DbObjectTypeKeys.ClusteredColumnstoreIndex:
						strSubObjectType = "5";
						break;
					case DbObjectTypeKeys.NonclusteredColumnstoreIndex:
						strSubObjectType = "6";
						break;
					case DbObjectTypeKeys.PrimaryXMLIndex:
					case DbObjectTypeKeys.SecondaryXMLIndex:
						strSubObjectType = "3";
						break;
					case DbObjectTypeKeys.SpatialIndex:
						strSubObjectType = "4";
						break;
					}
					strSubObjectCondition += "\n\tAND [type] = " + strSubObjectType;
					strSubObjectTableName = "sys.indexes";
					strSubObjectIDColumnName = "index_id";
					break;
				case DbObjectTypeKeys.Trigger:
					strSubObjectTableName = "sys.all_objects";
					strObjectIDColumnName = "parent_object_id";
					strSubObjectCondition += "\n\tAND [type] = 'TR'";
					break;
				}
				strCommandText =
				"USE " + strDBName + @"
				SELECT
					" + strDB_ID + @"[database_id],
					" + strObjectIDColumnName + @"[parent_object_id],
					" + strSubObjectIDColumnName + @"[id],
					" + strSubObjectNameColumnName + @"[name]
				FROM
					" + strDBName + "." + strSubObjectTableName + @"
				WHERE
					" + strObjectIDColumnName + " = " + item.ID.ToString()
					+ strSubObjectCondition;
			}
			return strCommandText;
		}
		public void AppendDBObjectsFromSqlQuery(DbObject parent, DbObjectTypeKeys dbobjectTypeKey, System.String strCommandText)
		{
			if(parent == null || (int)dbobjectTypeKey == 0 || System.String.IsNullOrEmpty(strCommandText))
			{
				System.String strParamName = "";
				if(parent == null)
				{
					strParamName += ", parent";
				}
				if((int)dbobjectTypeKey == 0)
				{
					strParamName += ", dbobjectTypeKey";
				}
				if(System.String.IsNullOrEmpty(strCommandText))
				{
					strParamName += ", strCommandText";
				}
				strParamName = strParamName.Substring(2);
				System.ArgumentNullException e = new System.ArgumentNullException(strParamName, "AppendDBObjectsFromSqlQuery argument(s) '" + strParamName + "' does not specified.");
				throw e;
			}
			if(parent.Children == null)
			{
				System.ArgumentException e = new System.ArgumentException("AppendDBObjectsFromSqlQuery argument 'parent' invalid: 'Children' is not set", "parent");
				throw e;
			}
			if(EstablishConnection())
			{
				strCommandText += @"
				ORDER BY
					[name]";
				System.Data.DataSet ds = null;
				System.Data.SqlClient.SqlDataAdapter adapter = null;
				System.String strName = null;
				try
				{
					ds = new System.Data.DataSet();
					adapter = new System.Data.SqlClient.SqlDataAdapter(strCommandText, SqlConnection);
					adapter.Fill(ds);
					if(ds.Tables != null && ds.Tables.Count == 1 && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
					{
						foreach(System.Data.DataRow dataRow in ds.Tables[0].Rows)
						{
							strName = (System.String)dataRow["name"];
							if(!System.String.IsNullOrEmpty(strName))
							{
								parent.Children.Add(new DbObject(strName, dbobjectTypeKey, parent));
								parent.Children[parent.Children.Count - 1].ID = ds.Tables[0].Columns.Contains("id") ? System.Convert.ToInt32(dataRow["id"]) : 0;
								parent.Children[parent.Children.Count - 1].DatabaseID =
									ds.Tables[0].Columns.Contains("database_id") ? System.Convert.ToInt32(dataRow["database_id"]) : 0;
								parent.Children[parent.Children.Count - 1].ParentObjectID =
									ds.Tables[0].Columns.Contains("parent_object_id") ? System.Convert.ToInt32(dataRow["parent_object_id"]) : 0;
								parent.Children[parent.Children.Count - 1].IsSystemObject =
									ds.Tables[0].Columns.Contains("is_system_object") && System.Convert.ToBoolean(dataRow["is_system_object"]);
								parent.Children[parent.Children.Count - 1].IsFixedRole =
									ds.Tables[0].Columns.Contains("is_fixed_role") && System.Convert.ToBoolean(dataRow["is_fixed_role"]);
							}
						}
					}
				}
				catch(System.Exception e)
				{
					App.ShowErrorMessage(e);
					throw e;
				}
			}
		}
		public bool FillDataGridFromSqlQuery(Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGrid, System.String strCommandText)
		{
			return FillDataGridFromSqlQuery(dataGrid, strCommandText, System.Data.CommandType.Text);
		}
		public bool FillDataGridFromSqlQuery(Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGrid, System.String strCommandText, System.Data.CommandType commandType)
		{
			if(dataGrid != null && !System.String.IsNullOrEmpty(strCommandText) && SqlConnection != null && SqlConnection.State == System.Data.ConnectionState.Open)
			{
				dataGrid.Columns.Clear();
				dataGrid.ItemsSource = null;
				System.Data.DataSet ds = null;
				System.Data.SqlClient.SqlDataAdapter adapter = null;
				System.Data.SqlClient.SqlCommand command = null;
				System.String strValue = null;
				try
				{
					ds = new System.Data.DataSet();
					adapter = new System.Data.SqlClient.SqlDataAdapter(strCommandText, SqlConnection);
					command = new System.Data.SqlClient.SqlCommand(strCommandText, SqlConnection);
					if(adapter != null && command != null)
					{
						command.CommandType = commandType;
						adapter.SelectCommand = command;
						adapter.Fill(ds);
					}
				}
				catch(System.Exception e)
				{
					App.ShowErrorMessage(e);
					throw e;
				}
				if(ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Count > 0)
				{
					Microsoft.Toolkit.Uwp.UI.Controls.DataGridTextColumn curDataGridColumn = null;
					int nIndex = 0;
					foreach(System.Data.DataColumn dataColumn in ds.Tables[0].Columns)
					{
						if(dataColumn != null)
						{
							nIndex = ds.Tables[0].Columns.IndexOf(dataColumn);
							curDataGridColumn = new Microsoft.Toolkit.Uwp.UI.Controls.DataGridTextColumn()
							{
								Header = dataColumn.ColumnName,
								Binding = new Windows.UI.Xaml.Data.Binding() { Path = new Windows.UI.Xaml.PropertyPath("[" + nIndex.ToString() + "]") }
							};
							dataGrid.Columns.Add(curDataGridColumn);
						}
					}
					System.Collections.ObjectModel.ObservableCollection<System.Object> itemsSource = new System.Collections.ObjectModel.ObservableCollection<System.Object>();
					foreach(System.Data.DataRow dataRow in ds.Tables[0].Rows)
					{
						if(dataRow != null)
						{
							itemsSource.Add(dataRow.ItemArray);
						}
					}
					dataGrid.ItemsSource = itemsSource;
				}
			}
			return dataGrid != null && dataGrid.ItemsSource != null;
		}
		public void ClearItems(System.Collections.ObjectModel.ObservableCollection<DbObject> itemsSource)
		{
			if(itemsSource != null)
			{
				System.Collections.Generic.IEnumerator<DbObject> enumTVItems = itemsSource.GetEnumerator();
				while(enumTVItems.MoveNext())
				{
					if(enumTVItems.Current != null && enumTVItems.Current.Children != null)
					{
						ClearItems(enumTVItems.Current.Children);
					}
				}
				itemsSource.Clear();
			}
		}
		public static void AddMenuFlyoutItemsMapping(Windows.UI.Xaml.DependencyObject dpobj,
			System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.Controls.MenuFlyoutItemBase> dictMenuItems)
		{
			if(dpobj != null && dictMenuItems != null && (dpobj is Windows.UI.Xaml.Controls.MenuFlyout || dpobj is Windows.UI.Xaml.Controls.MenuFlyoutSubItem))
			{
				System.Collections.Generic.IList<Windows.UI.Xaml.Controls.MenuFlyoutItemBase> listMenuItems = dpobj is Windows.UI.Xaml.Controls.MenuFlyoutSubItem ?
					((Windows.UI.Xaml.Controls.MenuFlyoutSubItem)dpobj).Items : ((Windows.UI.Xaml.Controls.MenuFlyout)dpobj).Items;
				Windows.UI.Xaml.Input.XamlUICommand curCommand = null;
				System.String strKey = null;
				foreach(Windows.UI.Xaml.Controls.MenuFlyoutItemBase curMenuItem in System.Linq.Enumerable.Where<Windows.UI.Xaml.Controls.MenuFlyoutItemBase>(listMenuItems,
					curitem => curitem != null && !(curitem is Windows.UI.Xaml.Controls.MenuFlyoutSeparator)))
				{
					if(curMenuItem is Windows.UI.Xaml.Controls.MenuFlyoutSubItem)
					{
						strKey = ((Windows.UI.Xaml.Controls.MenuFlyoutSubItem)curMenuItem).Text.IndexOf("script") > 0 ? "ScriptMenu" : "";
					}
					else
					{
						curCommand = (Windows.UI.Xaml.Input.XamlUICommand)((Windows.UI.Xaml.Controls.MenuFlyoutItem)curMenuItem).Command;
						if(curCommand != null)
						{
							strKey = curCommand.Label;
						}
					}
					if(!System.String.IsNullOrEmpty(strKey))
					{
						dictMenuItems.Add(strKey, curMenuItem);
					}
					if(curMenuItem is Windows.UI.Xaml.Controls.MenuFlyoutSubItem)
					{
						AddMenuFlyoutItemsMapping(curMenuItem, dictMenuItems);
					}
				}
			}
		}
		public void ShowCommandBorder()
		{
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridRowset = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["RowsetDataGrid"];
			Windows.UI.Xaml.Controls.Border borderCommand = (Windows.UI.Xaml.Controls.Border)DialogControls["CommandBorder"];
			dataGridRowset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			borderCommand.Visibility = Windows.UI.Xaml.Visibility.Visible;
		}
		public void ShowRowsetDataGrid()
		{
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridRowset = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["RowsetDataGrid"];
			Windows.UI.Xaml.Controls.Border borderCommand = (Windows.UI.Xaml.Controls.Border)DialogControls["CommandBorder"];
			dataGridRowset.Visibility = Windows.UI.Xaml.Visibility.Visible;
			borderCommand.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}
		public void ShowResultDataGrid()
		{
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridRowset = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["RowsetDataGrid"];
			Windows.UI.Xaml.Controls.Border borderCommand = (Windows.UI.Xaml.Controls.Border)DialogControls["CommandBorder"];
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridResult = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["ResultDataGrid"];
			dataGridRowset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			Windows.UI.Xaml.Controls.Grid.SetRowSpan(borderCommand, 0);
			borderCommand.Visibility = Windows.UI.Xaml.Visibility.Visible;
			dataGridResult.Visibility = Windows.UI.Xaml.Visibility.Visible;
		}
		public void HideResultDataGrid()
		{
			Windows.UI.Xaml.Controls.Border borderCommand = (Windows.UI.Xaml.Controls.Border)DialogControls["CommandBorder"];
			Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridResult = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["ResultDataGrid"];
			if(borderCommand.Visibility == Windows.UI.Xaml.Visibility.Visible)
			{
				dataGridResult.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
				Windows.UI.Xaml.Controls.Grid.SetRowSpan(borderCommand, 3);
			}
		}
		public void ExecuteCommandAndShowResult()
		{
			Windows.UI.Xaml.Controls.TextBox textboxCommand = (Windows.UI.Xaml.Controls.TextBox)DialogControls["CommandTextBox"];
			if(!System.String.IsNullOrEmpty(textboxCommand.Text))
			{
				if(EstablishConnection())
				{
					Microsoft.Toolkit.Uwp.UI.Controls.DataGrid dataGridResult = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)DialogControls["ResultDataGrid"];
					if(FillDataGridFromSqlQuery(dataGridResult, textboxCommand.Text))
					{
						ShowResultDataGrid();
					}
				}
			}
		}
		public static System.Object TryGetResource(System.String strResourceKey)
		{
			System.Object resource = null;
			if(!System.String.IsNullOrEmpty(strResourceKey))
			{
				SqlDbContentControl contentControl = Window.Current.Content as SqlDbContentControl;
				if(contentControl != null && contentControl.Resources != null && contentControl.Resources.ContainsKey(strResourceKey))
				{
					resource = contentControl.Resources[strResourceKey];
				}
				else if(Application.Current.Resources != null && Application.Current.Resources.ContainsKey(strResourceKey))
				{
					resource = Application.Current.Resources[strResourceKey];
				}
			}
			return resource;
		}
	}
}
