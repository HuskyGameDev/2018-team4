using System;
using System.Collections.Generic;

namespace BoardGameScripting.CSL {
	public class Terminals {
		public class COMMENT : GrammarElement {
			public override string GetRegex() {
				return @"^(/\*(.*)\*/)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}

		public class NEWLN : GrammarElement {
			public override string GetRegex() {
				return @"^(\n)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}

		public class WHITESP : GrammarElement {
			public override string GetRegex() {
				return @"^\s$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}

		public class WHILE : GrammarElement {
			public override string GetRegex() {
				return @"^(while)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ELSE : GrammarElement {
			public override string GetRegex() {
				return @"^(else)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class IF : GrammarElement {
			public override string GetRegex() {
				return @"^(if)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class TEXT : GrammarElement {
			public override string GetRegex() {
				return @"^(text)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class VAR : GrammarElement {
			public override string GetRegex() {
				return @"^(var)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class TRUE : GrammarElement {
			public override string GetRegex() {
				return @"^(true)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}

		public class FALSE : GrammarElement {
			public override string GetRegex() {
				return @"^(false)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		
		public class IMAGE : GrammarElement {
			public override string GetRegex() {
				return @"^(image)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class IMMEDIATE : GrammarElement {
			public override string GetRegex() {
				return @"^((immediate)|(enter))$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class DELAYED : GrammarElement {
			public override string GetRegex() {
				return @"^((delayed)|(exit))$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class STATIC : GrammarElement {
			public override string GetRegex() {
				return @"^(static)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class NAME : GrammarElement {
			public override string GetRegex() {
				return @"^(name)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class OPTIONAL : GrammarElement {
			public override string GetRegex() {
				return @"^(optional)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class REQUIRED : GrammarElement {
			public override string GetRegex() {
				return @"^(required)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ROOM : GrammarElement {
			public override string GetRegex() {
				return @"^(room)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ITEM : GrammarElement {
			public override string GetRegex() {
				return @"^(item)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}

		public class EVENT : GrammarElement {
			public override string GetRegex() {
				return @"^(event)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ARTIFACT : GrammarElement {
			public override string GetRegex() {
				return @"^(artifact)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class CHOICEIDEN : GrammarElement {
			public override string GetRegex() {
				return @"^(choice)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class EFFECTIDEN : GrammarElement {
			public override string GetRegex() {
				return @"^(effect)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class DISCARD : GrammarElement {
			public override string GetRegex() {
				return @"^(discard)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class THIS : GrammarElement {
			public override string GetRegex() {
				return @"^(this)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class LEFTBRACE : GrammarElement {
			public override string GetRegex() {
				return @"^\{$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class LEFTSQR : GrammarElement {
			public override string GetRegex() {
				return @"^\[$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class LEFTPAREN : GrammarElement {
			public override string GetRegex() {
				return @"^\($";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class RIGHTBRACE : GrammarElement {
			public override string GetRegex() {
				return @"^\}$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class RIGHTSQR : GrammarElement {
			public override string GetRegex() {
				return @"^\]$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class RIGHTPAREN : GrammarElement {
			public override string GetRegex() {
				return @"^\)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class AND : GrammarElement {
			public override string GetRegex() {
				return @"^(&&)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class OR : GrammarElement {
			public override string GetRegex() {
				return @"^(\|\|)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class XOR : GrammarElement {
			public override string GetRegex() {
				return @"^((\|)(x)(\|))$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class NOT : GrammarElement {
			public override string GetRegex() {
				return @"^(\!)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class COMMA : GrammarElement {
			public override string GetRegex() {
				return @"^(,)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class PERIOD : GrammarElement {
			public override string GetRegex() {
				return @"^(\.)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class SEMICOLON : GrammarElement {
			public override string GetRegex() {
				return @"^(;)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class EQUAL : GrammarElement {
			public override string GetRegex() {
				return @"^(==)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class NOTEQUAL : GrammarElement {
			public override string GetRegex() {
				return @"^(!=)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class GREATEQUAL : GrammarElement {
			public override string GetRegex() {
				return @"^(>=)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class LESSEQUAL : GrammarElement {
			public override string GetRegex() {
				return @"^(<=)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class GREATTHAN : GrammarElement {
			public override string GetRegex() {
				return @"^(>)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class LESSTHAN : GrammarElement {
			public override string GetRegex() {
				return @"^(<)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ASSIGN : GrammarElement {
			public override string GetRegex() {
				return @"^(=)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ADD : GrammarElement {
			public override string GetRegex() {
				return @"^(\+)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class SUB : GrammarElement {
			public override string GetRegex() {
				return @"^(-)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class DIV : GrammarElement {
			public override string GetRegex() {
				return @"^(/)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class MUL : GrammarElement {
			public override string GetRegex() {
				return @"^(\*)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class IDENERR : GrammarElement {
			public override string GetRegex() {
				return @"^([0-9]+[a-zA-Z]([a-zA-Z]|[0-9])+)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class IDENTIFIER : GrammarElement {
			public override string GetRegex() {
				return "^([a-zA-Z]([a-zA-Z]|[0-9])*)$"; // @ misstion is not a mistake
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class STRINGCON : GrammarElement {
			public override string GetRegex() {
				return "^(\"([^\"]*)\")$"; // @ missing is not a mistake
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class FLOATCON : GrammarElement {
			public override string GetRegex() {
				return @"^((-?)(\d)*(\.)(\d)+)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class INTCON : GrammarElement {
			public override string GetRegex() {
				return @"^(-?[0-9]+)$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
		public class ERR : GrammarElement {
			public override string GetRegex() {
				return @"^.*$";
			}

			public override TokenType GetTokenType() {
				return TokenType.Terminal;
			}
		}
	}
}