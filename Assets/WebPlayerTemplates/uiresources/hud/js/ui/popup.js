game.popup = {
	show : function(options)
	{
		var el = $("<div id='popup'></div>");
		el.prependTo('#hud');

		var docHeight = document.documentElement.clientHeight;
		var boxHeight = docHeight - 200;
		
		el.show();

		
			el.css({
				marginLeft: el.width() / 2 * -1,
				height: boxHeight,
				marginTop : (boxHeight / 2) * -1
			});	
		
		

		return el;
	}
}