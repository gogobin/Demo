
using System.Collections.Generic;

namespace ShowMeTheXAML
{
    public static class XamlDictionary
    {
        static XamlDictionary()
        {
            XamlResolver.Set("cards_7", @"<smtx:XamlDisplay UniqueKey=""cards_7"" Margin=""4 4 0 0"" VerticalContentAlignment=""Top"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:Flipper Style=""{StaticResource MaterialDesignCardFlipper}"" IsFlippedChanged=""Flipper_OnIsFlippedChanged"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <materialDesign:Flipper.FrontContent>
      <Button Style=""{StaticResource MaterialDesignFlatButton}"" Command=""{x:Static materialDesign:Flipper.FlipCommand}"" Margin=""8"" Width=""184"" Content=""FLIPPABLZ!"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" />
    </materialDesign:Flipper.FrontContent>
    <materialDesign:Flipper.BackContent>
      <Button Style=""{StaticResource MaterialDesignFlatButton}"" Command=""{x:Static materialDesign:Flipper.FlipCommand}"" Margin=""8"" Width=""184"" Content=""GO BACK"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" />
    </materialDesign:Flipper.BackContent>
  </materialDesign:Flipper>
</smtx:XamlDisplay>");
XamlResolver.Set("cards_8", @"<smtx:XamlDisplay UniqueKey=""cards_8"" Margin=""4 4 0 0"" VerticalContentAlignment=""Top"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:Flipper Style=""{StaticResource MaterialDesignCardFlipper}"" materialDesign:ShadowAssist.ShadowDepth=""Depth0"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <materialDesign:Flipper.FrontContent>
      <Grid Height=""256"" Width=""200"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <Grid.RowDefinitions>
          <RowDefinition Height=""160"" />
          <RowDefinition Height=""*"" />
        </Grid.RowDefinitions>
        <materialDesign:ColorZone Mode=""PrimaryLight"" VerticalAlignment=""Stretch"">
          <materialDesign:PackIcon Kind=""AccountCircle"" Height=""128"" Width=""128"" VerticalAlignment=""Center"" HorizontalAlignment=""Center"" />
        </materialDesign:ColorZone>
        <StackPanel Grid.Row=""1"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
          <TextBlock Text=""James Willock"" />
          <Button Style=""{StaticResource MaterialDesignFlatButton}"" Command=""{x:Static materialDesign:Flipper.FlipCommand}"" Margin=""0 4 0 0"" Content=""EDIT"" />
        </StackPanel>
      </Grid>
    </materialDesign:Flipper.FrontContent>
    <materialDesign:Flipper.BackContent>
      <Grid Height=""256"" Width=""200"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <Grid.RowDefinitions>
          <RowDefinition Height=""Auto"" />
          <RowDefinition Height=""*"" />
        </Grid.RowDefinitions>
        <materialDesign:ColorZone Padding=""6"">
          <StackPanel Orientation=""Horizontal"">
            <Button Style=""{StaticResource MaterialDesignToolForegroundButton}"" Command=""{x:Static materialDesign:Flipper.FlipCommand}"" HorizontalAlignment=""Left"">
              <materialDesign:PackIcon Kind=""ArrowLeft"" HorizontalAlignment=""Right"" />
            </Button>
            <TextBlock Margin=""8 0 0 0"" VerticalAlignment=""Center"" Text=""EDIT USER"" />
          </StackPanel>
        </materialDesign:ColorZone>
        <Grid Grid.Row=""1"" Margin=""0 6 0 0"" HorizontalAlignment=""Center"" VerticalAlignment=""Top"" Width=""172"">
          <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
            <RowDefinition />
            <RowDefinition />
          </Grid.RowDefinitions>
          <TextBox materialDesign:HintAssist.Hint=""First name"" materialDesign:HintAssist.IsFloating=""True"" Margin=""0 12 0 0"" Text=""James"" />
          <TextBox Grid.Row=""1"" materialDesign:HintAssist.Hint=""Last name"" materialDesign:HintAssist.IsFloating=""True"" Margin=""0 12 0 0"" Text=""Willock"" />
          <StackPanel Grid.Row=""2"" Orientation=""Horizontal"" Margin=""0 12 0 0"" HorizontalAlignment=""Right"">
            <TextBlock VerticalAlignment=""Center"" Text=""Email Contact"" />
            <ToggleButton Margin=""8 0 0 0"" />
          </StackPanel>
          <StackPanel Grid.Row=""3"" Orientation=""Horizontal"" Margin=""0 12 0 0"" HorizontalAlignment=""Right"">
            <TextBlock VerticalAlignment=""Center"" Text=""Telephone Contact"" />
            <ToggleButton Margin=""8 0 0 0"" />
          </StackPanel>
        </Grid>
      </Grid>
    </materialDesign:Flipper.BackContent>
  </materialDesign:Flipper>
</smtx:XamlDisplay>");
        }
    }
}