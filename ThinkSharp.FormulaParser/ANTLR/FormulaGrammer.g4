grammar FormulaGrammer;

formula
	:   expression EOF					
	;

expression															
   : multiplyingExpression ((PLUS | MINUS) multiplyingExpression)*
   ;

multiplyingExpression
   : powExpression ((TIMES | DIV) powExpression)*
   ;

powExpression														
   : signedAtom (POW signedAtom)*
   ;

signedAtom
   : PLUS atom																# PlusAtom
   | MINUS atom																# NegativeAtom
   | func																	# Function
   | atom																	# UnsignedAtom
   ;

atom
   : scientific
   | variable
   | LPAREN expression RPAREN
   ;

scientific															
   : SCIENTIFIC_NUMBER														# ScientificNumber
   ;

func																
   : IDENTIFIER LPAREN (expression (COMMA expression)*)? RPAREN
   ;

variable															
   : IDENTIFIER
   ;


LPAREN
   : '('
   ;


RPAREN
   : ')'
   ;


PLUS
   : '+'
   ;


MINUS
   : '-'
   ;


TIMES
   : '*'
   ;


DIV
   : '/'
   ;
   

COMMA
   : ','
   ;


POINT
   : '.'
   ;


POW
   : '^'
   ;


IDENTIFIER
   : VALID_ID_START VALID_ID_CHAR*
   ;

SCIENTIFIC_NUMBER
   : NUMBER ((E1 | E2) SIGN? NUMBER)?
   ;

fragment VALID_ID_START
   : ('a' .. 'z') | ('A' .. 'Z') | '_'
   ;


fragment VALID_ID_CHAR
   : VALID_ID_START | ('0' .. '9')
   ;

fragment NUMBER
   : ('0' .. '9') + ('.' ('0' .. '9') +)?
   ;


fragment E1
   : 'E'
   ;


fragment E2
   : 'e'
   ;


fragment SIGN
   : ('+' | '-')
   ;


WS
   : [ \r\n\t] + -> skip
   ;