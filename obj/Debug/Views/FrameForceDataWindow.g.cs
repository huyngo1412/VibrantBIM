﻿#pragma checksum "..\..\..\Views\FrameForceDataWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9731E699898406ABB347D686A5A1B12DDB3AB103F5F78CF863EBDE0240E74BDF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using VibrantBIM.Views;


namespace VibrantBIM.Views {
    
    
    /// <summary>
    /// FrameForceDataWindow
    /// </summary>
    public partial class FrameForceDataWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid Dg_FrameForceData;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Lb_LoadCase;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Lb_FrameName;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Apply;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ExportEx;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tb_LinkCXV;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\Views\FrameForceDataWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_InsertLinkCXV;
        
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
            System.Uri resourceLocater = new System.Uri("/VibrantBIM;component/views/frameforcedatawindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\FrameForceDataWindow.xaml"
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
            this.Dg_FrameForceData = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.Lb_LoadCase = ((System.Windows.Controls.ListBox)(target));
            return;
            case 3:
            this.Lb_FrameName = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.btn_Apply = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btn_ExportEx = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.tb_LinkCXV = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.btn_InsertLinkCXV = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
