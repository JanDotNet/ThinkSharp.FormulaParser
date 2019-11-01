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
   | atom																	# UnsignedAtom
   ;

atom
   : number
   | prefixedNumber
   | variable
   | LPAREN expression RPAREN
   | func																	
   ;

number
   : DECIMAL_NUMBER															# DecimalNumber
   | INTEGER_NUMBER															# IntgerNumber
   ;

prefixedNumber
   : PREFIX_DEC_NUMBER														# PrefixedDecNumber
   | PREFIX_INT_NUMBER														# PrefixedIntNumber
   | PREFIX_BIN_NUMBER														# PrefixedBinNumber
   | PREFIX_OCT_NUMBER														# PrefixedOctNumber
   | PREFIX_HEX_NUMBER														# PrefixedHexNumber
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
   : NUMBER_DEC
   ;

INTEGER_NUMBER
   : NUMBER_INT
   ;

PREFIX_BIN_NUMBER
   : PREFIX_BIN NUMBER_BIN
   ;

PREFIX_HEX_NUMBER
   : PREFIX_HEX NUMBER_HEX
   ;

PREFIX_INT_NUMBER
   : PREFIX_INT NUMBER_INT
   ;
   
PREFIX_OCT_NUMBER
   : PREFIX_OCT NUMBER_OCT
   ;

PREFIX_DEC_NUMBER
   : PREFIX_DEC (NUMBER_DEC | NUMBER_INT)
   ;

fragment VALID_ID_START
   : ('a' .. 'z') | ('A' .. 'Z') | '_' | '$'
   ;
   
fragment VALID_ID_CHAR
   : VALID_ID_START | ('0' .. '9')
   ;

fragment NUMBER_INT
   : ('0' .. '9') + 
   ;
   
fragment NUMBER_OCT
   : ('0' .. '7') + 
   ;

fragment NUMBER_DEC
   : ('0' .. '9') * ('.' ('0' .. '9') +)
   ;

fragment NUMBER_BIN
   : ('0' | '1')+
   ;

fragment NUMBER_HEX
   : (('0' .. '9') | ('A' .. 'F') | ('a' .. 'f')) +
   ;

fragment PREFIX_DEC
   : '0' ('D' | 'd')
   ;

fragment PREFIX_INT
   : '0' ('I' | 'i')
   ;

fragment PREFIX_OCT
   : '0' ('o' | 'O')
   ;

fragment PREFIX_BIN
   : '0' ('B' | 'b')
   ;

fragment PREFIX_HEX
   : '0' ('X' | 'x')
   ;

fragment E
   : ('E' | 'e')
   ;

fragment SIGN
   : ('+' | '-')
   ;
   
WS
   : [ \r\n\t] + -> skip
   ;