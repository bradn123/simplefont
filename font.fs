( ------------------------------------------------------------ )

variable weight  5 weight !

: clear ( -- ) 0 0 pixel width height * 4 * 255 fill ;
: plot ( x y -- ) pixel 0 over c! 0 over 1+ c! 0 swap 2 + c! ;
: penplot ( x y -- )
  dup weight @ + swap weight @ - do
    dup weight @ + over weight @ - do
      i j plot
    loop
  loop
  drop
;

( ------------------------------------------------------------ )
: point   create 2 cells allot ;
: p! ( x y a -- ) swap over cell+ ! ! ;
: p@ ( a -- x y ) dup @ swap cell+ @ ;

: square ( n -- n ) dup * ;
: distance2 ( x1 y1 x2 y2 ) rot - square -rot - square + ;
: middle ( x1 y1 x2 y2 -- mx my ) rot + 2/ >r + 2/ r> ;

point q1  point q2  point q3  point q12  point q23  point q123

: quartic ( p1 p2 p3 -- )
  q3 p! q2 p! q1 p!
  q1 p@ q3 p@ distance2 10 < if
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

( ------------------------------------------------------------ )
( Font definition tools )
: /;,   99 c, ;
: /,   dup 10000 / c,  dup 100 / c,  c, ;


