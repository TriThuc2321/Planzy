﻿#pragma checksum "..\..\..\Views\PortWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F8E01903AA8AF9DDC2544CED4EE0AF3D4ECB79902205D0B33BA94841DFDBF45A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CustomMessageBox;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CustomMessageBox {
    
    
    /// <summary>
    /// PortWindow
    /// </summary>
    public partial class PortWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 73 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border NodeSelectionBorder;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTop;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CanvasNodeBaseLayerOne;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CaptionTxtBlock;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CLOSE;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MessageBoxIcon;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageBoxTextBlck;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition MessageCancelGrid;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageOk;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageCancel;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\Views\PortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageYes;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CustomMessageBox;component/views/portwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\PortWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\Views\PortWindow.xaml"
            ((CustomMessageBox.PortWindow)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NodeSelectionBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.gridTop = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.CanvasNodeBaseLayerOne = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            this.CaptionTxtBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.CLOSE = ((System.Windows.Controls.Button)(target));
            
            #line 91 "..\..\..\Views\PortWindow.xaml"
            this.CLOSE.Click += new System.Windows.RoutedEventHandler(this.CLOSE_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.MessageBoxIcon = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.MessageBoxTextBlck = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.MessageCancelGrid = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 10:
            this.MessageOk = ((System.Windows.Controls.Button)(target));
            
            #line 121 "..\..\..\Views\PortWindow.xaml"
            this.MessageOk.Click += new System.Windows.RoutedEventHandler(this.MessageOk_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.MessageCancel = ((System.Windows.Controls.Button)(target));
            
            #line 128 "..\..\..\Views\PortWindow.xaml"
            this.MessageCancel.Click += new System.Windows.RoutedEventHandler(this.MessageCancel_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.MessageYes = ((System.Windows.Controls.Button)(target));
            
            #line 135 "..\..\..\Views\PortWindow.xaml"
            this.MessageYes.Click += new System.Windows.RoutedEventHandler(this.MessageYes_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

