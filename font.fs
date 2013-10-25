( ------------------------------------------------------------ )
( Tools )
: square ( n -- n ) dup * ;
: sqrt ( n -- n ) s>f fsqrt f>s ;
: point   create 2 cells allot ;
: p! ( x y a -- ) swap over cell+ ! ! ;
: p@ ( a -- x y ) dup @ swap cell+ @ ;
: p+ ( p1 p2 -- p1+p2 ) rot + -rot + swap ;
: p- ( p1 p2 -- p1-p2 ) rot swap - -rot - swap ;
: plen2 ( p -- n ) square swap square + ;
: distance2 ( p1 p2 -- n ) p- plen2 ;
: p-rshift ( p n -- p' ) dup >r rshift swap r> rshift swap ;
: p-lshift ( p n -- p' ) dup >r lshift swap r> lshift swap ;
: middle ( p1 p2 -- pm ) p+ 1 p-rshift ;


( ------------------------------------------------------------ )
( Pen plotting )
8 constant granularity
1 granularity lshift constant granual
granual square constant granual2
: g>> ( p -- p' ) granularity p-rshift ;
: g<< ( p -- p' ) granularity p-lshift ;

variable weight  1 weight !

: clear ( -- ) 0 0 pixel width height * 4 * 255 fill ;
: byte-clip ( n -- n ) 0 max 255 min ;
: pattern ( x y -- ) plen2 sqrt weight @ granularity lshift - byte-clip ;
: blend1 ( v a -- ) dup c@ rot min swap c! ;
: blend ( v a -- ) 2dup blend1 2dup 1+ blend1 2 + blend1 ;
point pt  point ptg
: penplot ( x y -- )
  pt p!  pt p@ g>> ptg p!
  weight @ 2 + weight @ negate 1- do
    weight @ 2 + weight @ negate 1- do
      pt p@ ptg p@ i j p+ 2dup pixel >r
      g<< p- pattern r> blend
    loop
  loop
;

( ------------------------------------------------------------ )
point q1  point q2  point q3  point q12  point q23  point q123
: quartic' ( p1 p2 p3 -- )
  q3 p! q2 p! q1 p!
  q1 p@ q3 p@ distance2 granual2 2/ < if
    q1 p@ q3 p@ middle penplot
  else
    q1 p@ q2 p@ middle q12 p!
    q2 p@ q3 p@ middle q23 p!
    q12 p@ q23 p@ middle q123 p!
    q1 p@ q12 p@ q123 p@
    q123 p@ q23 p@ q3 p@
    recurse recurse
  then 
;
: quartic ( p1 p2 p3 -- )
  g<< >r >r g<< >r >r g<< r> r> r> r> quartic' ;

( ------------------------------------------------------------ )
( Font definition tools )
: /;,   here 3 - dup c@ 128 or swap c! ;
: /,   dup 10000 / 100 mod c,
       dup 100 / 100 mod c,
       100 mod c, ;
variable font  variable font-x  variable font-y
variable font-width    20 font-width !
variable font-height   40 font-height !
33 constant min-character  126 constant max-character
: char-clip ( n -- n ) min-character max max-character min ;
: char-offset ( n -- n ) char-clip min-character - ;
: char-end? ( n -- n ) 128 and 0<> ;
: char-skip ( a -- a ) begin dup 3 + swap c@ char-end? until ;
: char-start ( n -- a ) char-offset font @ swap 0 ?do char-skip loop ;
: stroke-code ( n -- c ) 127 and ;
: stroke-x ( n -- x ) 10 /   2 - font-width @ 3 */ font-x @ + ;
: stroke-y ( n -- y ) 10 mod 2 - font-height @ 6 */ negate font-y @ + ;
: stroke-pt ( n -- x y ) stroke-code dup stroke-x swap stroke-y ;
: stroke-draw ( a -- )
  dup c@ stroke-pt rot
  dup 1+ c@ stroke-pt rot
      2 + c@ stroke-pt quartic ;
: char-draw ( n -- ) dup 33 < over 126 > or if exit then
                     char-start begin dup c@ char-end? 0= while
                       dup stroke-draw 3 + repeat stroke-draw
                       font-width @ font-x +! ;

