   M S B A M L   `   `   `  ????S  O龙岩餐饮管理系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:龙岩餐饮管理系统龙岩餐饮管理系统  5      +    %龙岩餐饮管理系统.会员管理   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:龙岩餐饮管理系统     5      -   5          N? Title$  会员管理??5	       N? FontSize$ 13(?6)   .??5
      ]? .+?5   
   ?? $
??Left=?6   $	??150??6+   $??5q?67   .+?5      ?? -   5         ??Name$ btnNew??5      $??5q?6:   .??5      ??  ?? Orientation$ 
Horizontal=?6   .+?5      ?? $??32??6   $??32??6'   $??PNG/新建.png??63   ?? 5      $??新建??6    $??Center=?6*   5      5      ?? 5      -   5        $ btnEdit??5      $??5q?6<   $??1??6G   .??5      ?? $ 
Horizontal=?6   .+?5      ?? $??32??6   $??32??6'   $??PNG/修改.png??63   ?? 5      $??修改??6    $??Center=?6*   5      5      ?? 5      -   5        $ btnDel??5      $??2??6:   $??5q?6G   .??5      ?? $ 
Horizontal=?6   .+?5      ?? $??32??6   $??32??6'   $??PNG/删除.png??63   ?? 5      $??删除??6    $??Center=?6*   5      5      ?? 5      -   5        $ 
btnRefresh??5      $??3??6B   $??5q?6O   .??5      ?? $ 
Horizontal=?6   .+?5       ?? $??32??6   $??32??6'   $??PNG/刷新.png??63   ?? 5!      $??刷新??6    $??Center=?6*   5"      5#      5$      &   System.Windows.Controls.DataGrid  5%   
   -   5          Name$ 	dgMembers??5%      $??5q?6&      
IsReadOnly .    AutoGenerateColumns .     SelectionMode$ Single=?6_      	RowHeight$ 22Q?6v   	   Columns	 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn
   Header$
 会员编码??6>      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ?? *Code+6L     5(      $
 会员名称??6>   $	 150 6L    6%   ?? *Name+6L     5)      $
 会员电话??6@   $	 120 6N    6%   ?? *Mobile+6N     5*      $
 余额??6A   $ 80 6M    6%   ?? *	Balance+   HeaderStyle 5+      ?? 5,       ?? 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .?5-      ??  ?? Property ? ?? ?? Value$ Right=?6K   5.      50        51      $
 积分??6B   $ 80 6N    6%   ?? *
Integral+ 52      ?? 53      "  6    .?54      ??  ? ??$ Right=?6K   55      57        58      $
 创建日期??6E   $	 100 6S    6%   ?? *Create_Date+ 59      ?? 5:      "  6    .?5;      ??  ? ??$ Center=?6K   5<      5>      5@      5A      5B      