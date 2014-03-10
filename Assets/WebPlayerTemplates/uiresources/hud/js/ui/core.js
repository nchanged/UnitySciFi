window.ui = {

}
ui._overlayCallback = null;
ui.bindOverlayTap = function(callback)
{
	ui._overlayCallback = callback;
}
ui.initTransparency = function()
{
	document.getElementById("hud").couiInputCallback = function()
	{
		if (ui._overlayCallback){
			var result = ui._overlayCallback();
			if ( result ){
				ui._overlayCallback = null;
			}
			return true;
		}
		return false;
	}
}