( ------------------------------------------------------------ )
256 constant blend
blend 1- constant blend'

: lband ( x y -- )
  over blend mod 65535 blend' */ gray pen
  swap blend / swap plot
;

: rband ( x y -- )
  over blend mod blend' swap - 65535 blend' */ gray pen
  swap blend / 1- swap plot
;

: band ( x y w -- )
  black pen >r 2dup swap blend / swap r> dup >r blend / 1 rect r>
  >r 2dup swap r> + swap rband
  lband
;
  

( ------------------------------------------------------------ )
: point   create 2 cells allot ;
: p! ( x y a -- ) swap over cell+ ! ! ;
: p@ ( a -- x y ) dup @ swap cell+ @ ;
: x@ ( a -- x ) @ ;
: y@ ( a -- y ) cell+ @ ;

: square ( n -- n ) dup * ;
: distance2 ( x1 y1 x2 y2 ) rot - square -rot - square + ;
: middle ( x1 y1 x2 y2 -- mx my ) rot + 2/ >r + 2/ r> ;
: middle' ( x1 y1 x2 y2 -- mx my ) rot + 1+ 2/ >r + 1+ 2/ r> ;
point q1  point q2  point q3  point q12  point q23  point q123
point q12' point q23' point q123'
: sp space ;
: quartic ( p1 p2 p3 -- )
  q3 p! q2 p! q1 p!
  q1 y@ q3 y@ - 0= if
    q1 p@ 2000 band
  else
    q1 p@ q2 p@ middle q12 p!
    q2 p@ q3 p@ middle q23 p!
    q1 p@ q2 p@ middle' q12' p!
    q2 p@ q3 p@ middle' q23' p!
    q12 p@ q23' p@ middle q123 p!
    q12' p@ q23 p@ middle' q123' p!
    q1 p@ q12 p@ q123 p@
    q123' p@ q23' p@ q3 p@
    recurse recurse
  then 
;

: line ( p1 p2 -- )
  q2 p! q1 p!
  q1 p@ q2 p@ distance2 2 < if
    q1 p@ 10 1 rect
  else
    q1 p@ q2 p@ middle q12 p!
    q1 p@ q2 p@ middle' q12' p!
    q1 p@ q12 p@
    q12 p@ q2 p@
    recurse recurse
  then
;
