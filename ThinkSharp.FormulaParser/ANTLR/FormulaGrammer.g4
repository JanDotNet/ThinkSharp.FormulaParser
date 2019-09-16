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
   : number
   | variable
   | LPAREN expression RPAREN
   ;

number															
   : SCIENTIFIC_NUMBER														# ScientificNumber
   | DECIMAL_NUMBER															# DecimalNumber
   | BINARY_NUMBER															# BinaryNumber
   | HEXADECIMAL_NUMBER														# HexadecimalNumber
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

DECIMAL_NUMBER
   : (PREFIX_DEC)? NUMBER_DEC
   ;

BINARY_NUMBER
   : PREFIX_BIN NUMBER_BIN
   ;

HEXADECIMAL_NUMBER
   : (PREFIX_HEX | NUMBER_HEX2) NUMBER_HEX
   ;

SCIENTIFIC_NUMBER
   : NUMBER_DEC (E1 | E2) SIGN? NUMBER_DEC
   ;

fragment VALID_ID_START
   : ('a' .. 'z') | ('A' .. 'Z') | '_'
   ;
   
fragment VALID_ID_CHAR
   : VALID_ID_START | ('0' .. '9')
   ;

fragment PREFIX_DEC
   : '0' ('D' | 'd')
   ;

fragment NUMBER_DEC
   : ('0' .. '9') + ('.' ('0' .. '9') +)?
   ;

fragment PREFIX_BIN
   : '0' ('B' | 'b')
   ;

fragment NUMBER_BIN
   : ('0' | '1')+
   ;

fragment PREFIX_HEX
   : '0' ('X' | 'x')
   ;

fragment NUMBER_HEX2
   : '#'
   ;

fragment NUMBER_HEX
   : (('0' .. '9') | ('A' .. 'F') | ('a' .. 'f')) +
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