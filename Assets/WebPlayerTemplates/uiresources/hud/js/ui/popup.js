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

		

		setTimeout(function(){
			options.opened(el);
		},1);
		

		ui.bindOverlayTap(function(){
			//move(el[0]).ease('out').duration(300).set("opacity",0).end(function(){
				
				el.remove();
			//});

			
			return true;
		});
		

		return el;
	}
}