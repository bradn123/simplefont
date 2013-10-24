( ------------------------------------------------------------ )

: square ( n -- n ) dup * ;
: sqrt ( n -- n ) s>f fsqrt f>s ;

8 constant granularity
1 granularity lshift constant granual
granual square constant granual2

variable weight  6 granularity lshift weight !


: clear ( -- ) 0 0 pixel width height * 4 * 255 fill ;
: byte-clip ( n -- n ) 0 max 255 min ;
: pattern ( x y -- )
  square swap square + sqrt weight @ granual - - byte-clip ;
: blend1 ( v a -- ) dup c@ rot min swap c! ;
: blend ( v a -- ) 2dup blend1 2dup 1+ blend1 2 + blend1 ;
: penplot ( x y -- )
  weight @ 1+ weight @ negate do
    weight @ 1+ weight @ negate do
      2dup j + granularity rshift swap
           i + granularity rshift swap pixel
      i j pattern swap blend
    granual +loop
  granual +loop
  2drop
;

( ------------------------------------------------------------ )
: point   create 2 cells allot ;
: p! ( x y a -- ) swap over cell+ ! ! ;
: p@ ( a -- x y ) dup @ swap cell+ @ ;

: distance2 ( x1 y1 x2 y2 ) rot - square -rot - square + ;
: middle ( x1 y1 x2 y2 -- mx my ) rot + 2/ >r + 2/ r> ;

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
  granularity lshift >r granularity lshift >r granularity lshift >r
  granularity lshift >r granularity lshift >r granularity lshift >r 
  r> r> r> r> r> r>
  quartic'
;

( ------------------------------------------------------------ )
( Font definition tools )
: /;,   here 3 - dup c@ 128 or swap c! ;
: /,   dup 10000 / 100 mod c,
       dup 100 / 100 mod c,
       100 mod c, ;
variable font
variable font-x
variable font-y
33 constant min-character  126 constant max-character
: char-clip ( n -- n ) min-character max max-character min ;
: char-offset ( n -- n ) char-clip min-character - ;
: char-end? ( n -- n ) 128 and 0<> ;
: char-skip ( a -- a ) begin dup 3 + swap c@ char-end? until ;
: char-start ( n -- a ) char-offset font @ swap 0 ?do char-skip loop ;
: stroke-code ( n -- c ) 127 and ;
: stroke-x ( n -- x ) 10 /   2 - 30 * 100 + font-x @ + ;
: stroke-y ( n -- y ) 10 mod 2 - 32 * negate 400 + font-y @ + ;
: stroke-pt ( n -- x y ) stroke-code dup stroke-x swap stroke-y ;
: stroke-draw ( a -- )
  dup c@ stroke-pt rot dup 1+ c@ stroke-pt rot 2 + c@ stroke-pt
  quartic ;
: char-draw ( n -- ) char-start begin dup c@ char-end? 0= while
                       dup stroke-draw 3 + repeat stroke-draw
                       100 font-x +! ;
