var templates = {
	getBuilding : function()
	{
		return $("<div class='unit'></div>");
	}
}
game.Menu = {
	OpenProbMenu: function(config) {
	
		var docHeight = document.documentElement.clientHeight;
		var boxHeight = docHeight - 200;
		var el = game.popup.show();	


		var data = $("#builder-menu-template").clone().show();
		el.html(data);
		el.find('.scrolling').css({height : el.height() - 70})


		for(var i =0; i<=5;i++){
			var tpl = $("#unit-template").clone().show();


			tpl.appendTo(el.find(".unit-list"));
		}

	}
}