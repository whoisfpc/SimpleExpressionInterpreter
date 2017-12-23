grammar Expr;
prog: expr EOF ;
expr:
    expr (MUL | DIV) expr # binaryExpr
    | expr (ADD | SUB) expr # binaryExpr
    | term # termExpr
    | LP expr RP #singleExpr
    ;
term:
    NUM
    | ID
    ;

fragment DIGIT: [0-9] ;

WS: [ \t\r\n]+ -> skip ;
NUM: DIGIT+ ('.' DIGIT+)? ;
ID: '$' DIGIT+ ;
ADD: '+' ;
SUB: '-' ;
MUL: '*' ;
DIV: '/' ;
LP: '(' ;
RP: ')' ;