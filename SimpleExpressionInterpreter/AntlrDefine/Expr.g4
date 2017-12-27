grammar Expr;

prog: expr EOF ;

expr:
    ID LP exprList? RP # funcExpr
    | expr (MUL | DIV) expr # binaryExpr
    | expr (ADD | SUB) expr # binaryExpr
    | term # termExpr
    | LP expr RP #singleExpr
    ;

exprList: expr (COMMA expr)* ;

term:
    NUM
    | PREVAR
    ;

fragment DIGIT: [0-9] ;
fragment LETTER : [a-zA-Z] ;

WS: [ \t\r\n]+ -> skip ;
NUM: DIGIT+ ('.' DIGIT+)? ;
ID: LETTER (LETTER | DIGIT)* ;
// predefine variable, fetch from out of context
PREVAR: '$' DIGIT+ ;
ADD: '+' ;
SUB: '-' ;
MUL: '*' ;
DIV: '/' ;
LP: '(' ;
RP: ')' ;
COMMA: ',' ;