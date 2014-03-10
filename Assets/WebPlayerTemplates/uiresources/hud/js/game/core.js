window.game =  {};
game.debug = function(data)
{
	$("#deb").show();
	$("#deb").html(JSON.stringify(data));

	setTimeout(function(){
		$("#deb").hide();
	},1000)
}
