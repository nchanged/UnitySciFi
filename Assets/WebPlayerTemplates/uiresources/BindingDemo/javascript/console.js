(function(window, $, engine, _) {
	'use strict';
	
	var gameConsole = {
		
		initialize : function () {
			var holder = '<div id="console"><div id="console_window" class="ui-corner-all"></div><input id="console_input" type="text" class="ui-corner-all"/></div>';
			$('body').append(holder);
			this.window = $('#console_window');
			this.input = $('#console_input');

			this._history = [];
			this._history_mode = false;
			this._history_index = 0;
			this._commands = {};
			this._commandNames = [];
			this._getCommand = /^([^\s]+)/;

			this.AddCommand('help', 'this help message', function () {
				gameConsole._printHelp();
			});

			this.input.autocomplete({ source : gameConsole._commandNames,
				appendTo: '#console',
				autoFocus: true,
				disabled: true,
				delay: 10
			});

			this.input.keypress((function (gameConsole) { return function(e) { return gameConsole._inputKeypress(e); }; }(this)));
			this.input.keydown((function (gameConsole) { return function(e) { return gameConsole._inputKeydown(e); }; }(this)));
			
			engine.trigger('gameConsole:registerCommands');
		},
		
		_commandTemplate	: _.template('<div class="command">&gt;&nbsp;<%= output %></div>'),
		_resultTemplate		: _.template('<div class="command_output"><%= output %></div>'),
		_errorTemplate		: _.template('<div class="command_output error"><%= output %></div>'),
		_helpTemplate		: _.template('<table class="command_list" cols="2"> \
		<% _.each(commands, function (c) { %>\
			<tr>\
				<td class="command_name"><%= c %></td><td class="command_desciption"><%= data[c].help %></td> \
			</tr><%})%>\
			<tr>\
				<td class="command_name">TAB</td><td class="command_description">activate autocomplete</td> \
			</tr>\
		</table>'),
		
		_traceTemplate    : _.template('<div class="command_trace"><%= output %></div>'),
		
		_printHelp : function () {
			var result = $(this._helpTemplate({commands: this._commandNames, data: this._commands}));
			this.window.append(result);
			this._scrollDown();
		},

		_stringify : function(value) {
			return (typeof(value) === "string")? value : JSON.stringify(value);
		},
		
		trace : function () {
			var args = Array.prototype.map.call(arguments, this._stringify).join(' ')
			this.append(args, this._traceTemplate);
		},


		append : function (output, template) {
			var window = this.window;
			var result = $(template({output: output}));
			window.append(result);
			this._scrollDown();
		},

		_appendCommand : function (command) {
			return this.append(command, this._commandTemplate);
		},
		
		appendResult : function (result) {
			return this.append(result, this._resultTemplate);
		},
		
		appendError : function (error) {
			return this.append(error, this._errorTemplate);
		},
		
		_engineCommand : function (name) {
			return function (gameConsole, name, line) {
				engine.call('gameConsole:' + name, line).then(
					_.bind(gameConsole.appendResult, gameConsole),
					_.bind(gameConsole.appendError, gameConsole)
				);
			};
		},
		
		MakeCommand : function (code) {
			return function (gameConsole, name, line) {
				try {
					var result = code(line);
					if (result !== undefined && result !== null) {
						gameConsole.appendResult(result);
					}
				}
				catch (error) {
					gameConsole.appendError(error.message);
				}
			};
		},
		
		Command : function (name, help, handler) {
			this.name = name;
			this.help = help;
			this.command = handler || gameConsole._engineCommand(name);
		},
		
		AddCommand : function (name, help, handler) {
			window.console.assert(this._commands[name] === undefined);
			this._commands[name] = new this.Command(name, help, this.MakeCommand(handler));
			this._commandNames.push(name);
			this._commandNames.sort();
			this.input.autocomplete('option', 'source', this._commandNames);
		},
		
		_toggle : function () {
			$('#console').slideToggle();
			this._scrollDown();
			this.input.focus();
			
		},
		
		_scrollDown : function () {
			gameConsole.window.scrollTop(1000000);
		},
		
		executeCommand : function (command) {
			if (command.length === 0) {
				return;
			}
			this._history.push(command);
			this._history_index = this._history.length;
			this._appendCommand(command);	
			
			var match = this._getCommand.exec(command),
				name = (match !== null)? match[1] : null;
			
			if (name !== null) {
				var handler = this._commands[name];
				if (handler) {
					handler.command(this, name, command);
				} else {
					this.appendError('no such command: ' + name);
				}
			} else {
				this.appendError('could not parse command name');
			}
			
			this.input.autocomplete('disable');
		},
		
		_setInput : function (value) {
			var input = this.input,
				element = input.get(0);
			input.val(value);
			element.focus();
			element.setSelectionRange(value.length, value.length);
		},
		
		_previousCommand : function () {
			if (gameConsole.input.autocomplete('option', 'disabled')) {
				var index = --this._history_index;
				if (index >= 0) {
					this._setInput(this._history[index]);
				} else {
					this._history_index = 0;
				}
			}
		},
		
		_nextCommand : function () {
			if (this.input.autocomplete('option', 'disabled')) {
				var index = ++this._history_index;
				if (index < this._history.length) {
					this._setInput(this._history[index]);
				}
				else {
					this._history_index = this._history.length - 1;
				}
			}
		},
		
		_autocomplete : function () {
			var input = this.input;
			input.autocomplete('enable');
			input.autocomplete('search', input.val());
		},

		_inputKeypress : function(e) {
			if (e.keyCode == 10 || e.keyCode == 13) {
				this.executeCommand(gameConsole.input.val());
				this.input.val('');
			}
		},

		_inputKeydown : function(e) {
			if (e.keyCode == 9) {
				this._autocomplete();
				e.preventDefault();
			} else if (e.keyCode == 38) {
				this._previousCommand();
				e.preventDefault();
			} else if (e.keyCode == 40) {
				this._nextCommand();
				e.preventDefault();
			} else if (e.KeyCode == 13) {
				this._inputKeypress(e);
			}
		}
	};
	
	if (!engine.isAttached) {
		$(window).keypress(function(e) {
			if (e.which == 96 || e.which == 126) {
				gameConsole._toggle();
				e.preventDefault();
			}
		});
	}
	
	engine.on('gameConsole:AddCommand', gameConsole.AddCommand, gameConsole);
	engine.on('gameConsole:Execute', gameConsole.executeCommand, gameConsole);
	engine.on('gameConsole:Trace', gameConsole.trace, gameConsole);
	
	engine.on('Ready', function () {
		gameConsole.initialize();
		
		engine.trigger('gameConsole:AddCommand', 'echo', 'echoes its args', function () {
			return JSON.stringify(arguments);
		});
		
		engine.trigger('gameConsole:AddCommand', 'eval', 'evals its args', function (line) {
			var code = /^[^\s]+\s+(.*)$/.exec(line)[1];
			return JSON.stringify(eval(code));
		});
	});
}(window, $, engine, _));
