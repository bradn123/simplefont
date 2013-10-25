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
: middle ( p1 p2 -- pm ) rot + 2/ -rot + 2/ swap ;
: byte-clip ( n -- n ) 0 max 255 min ;


( ------------------------------------------------------------ )
( Granulary of the plotting )
8 constant granularity
1 granularity lshift constant granual
granual square constant granual2
: g>> ( p -- p' ) granularity p-rshift ;
: g<< ( p -- p' ) granularity p-lshift ;


( ------------------------------------------------------------ )
( Weight of the font )
variable <>weight  variable (weight
: font-weight ( n -- )
  dup (weight !  granularity rshift 2 + <>weight ! ;
300 font-weight
: <->weight ( -- n -n ) <>weight @ 1+ <>weight @ negate ;


( ------------------------------------------------------------ )
( Pixel handling )
: clear ( -- ) 0 0 pixel width height * 4 * 255 fill ;

variable pixel-junk
: outside? ( x y -- f )
  2dup 0< swap 0< or >r height >= swap width >= or r> or ;
: pixel-clip ( x y -- a )
  2dup outside? if 2drop pixel-junk exit then pixel ;


( ------------------------------------------------------------ )
( Pen plotting )

: pattern ( x y -- ) plen2 sqrt (weight @ - byte-clip ;
: blend1 ( v a -- ) dup c@ rot min swap c! ;
: blend ( v a -- ) 2dup blend1 2dup 1+ blend1 2 + blend1 ;
point pt  point ptg

: penplot ( x y -- )
  pt p!  pt p@ g>> ptg p!
  <->weight do <->weight do
    pt p@ ptg p@ i j p+ 2dup pixel-clip >r
    g<< p- pattern r> blend
  loop loop ;


( ------------------------------------------------------------ )
( Quartic bezier curve plotting )
point q1  point q2  point q3  point q12  point q23  point q123
: quartic' ( p1 p2 p3 -- )
  q3 p! q2 p! q1 p!
  q1 p@ q3 p@ distance2 granual2 < if
    q1 p@ q3 p@ middle penplot
  else
    q1 p@ q2 p@ middle q12 p!
    q2 p@ q3 p@ middle q23 p!
    q12 p@ q23 p@ middle q123 p!
    q1 p@ q12 p@ q123 p@  q123 p@ q23 p@ q3 p@
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


( ------------------------------------------------------------ )
( Font layout properties )
variable font-margin  10 font-margin !
variable font  variable font-x  variable font-y
variable font-width    20 font-width !
variable font-height   40 font-height !


( ------------------------------------------------------------ )
( Finding a character )
33 constant min-character  126 constant max-character
: char-clip ( n -- n ) min-character max max-character min ;
: char-offset ( n -- n ) char-clip min-character - ;
: char-end? ( n -- n ) 128 and 0<> ;
: char-skip ( a -- a ) begin dup 3 + swap c@ char-end? until ;
: char-start ( n -- a )
  char-offset font @ swap 0 ?do char-skip loop ;


( ------------------------------------------------------------ )
( Decoding each character stroke )
: stroke-code ( n -- c ) 127 and ;
: stroke-x ( n -- x ) 10 /   2 - font-width @ 3 */ font-x @ + ;
: stroke-y ( n -- y )
  10 mod 2 - font-height @ 6 */ negate font-y @ + ;
: stroke-pt ( n -- x y )
  stroke-code dup stroke-x swap stroke-y ;
: stroke-draw ( a -- )
  dup c@ stroke-pt rot
  dup 1+ c@ stroke-pt rot
      2 + c@ stroke-pt quartic ;
: font-draw1 ( n -- )
  char-start begin dup c@ char-end? 0= while
  dup stroke-draw 3 + repeat stroke-draw ;


( ------------------------------------------------------------ )
( Text position handling )
: font-cr  font-margin @ font-x ! font-height @ font-y +! ;

: char-next   font-width @ font-x +!
  font-x @ font-width @ + width font-margin @ - >
  if font-cr then ;


( ------------------------------------------------------------ )
( Typing and control codes )

: font-emit ( n -- )
  dup 10 = if drop font-cr exit then
  dup 13 = if drop font-margin @ font-x ! exit then
  dup 8 = if drop font-width @ negate font-x +! exit then
  dup 33 < over 126 > or if drop char-next exit then
  font-draw1 char-next
;

: font-type ( a n -- ) over + swap ?do i c@ font-emit loop ;
