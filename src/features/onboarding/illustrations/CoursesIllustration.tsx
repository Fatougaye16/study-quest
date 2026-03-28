import React from 'react';
import Svg, { G, Rect, Path, Circle, Ellipse, Text as SvgText } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function CoursesIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  const books = [
    { x: 85, y: 120, w: 45, h: 100, color: primaryColor, label: 'Math' },
    { x: 110, y: 110, w: 42, h: 110, color: secondaryColor, label: 'Sci' },
    { x: 137, y: 125, w: 40, h: 95, color: accentColor, label: 'Eng' },
    { x: 160, y: 115, w: 43, h: 105, color: primaryColor, label: 'Hist' },
  ];

  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Shelf shadow */}
      <Ellipse cx="140" cy="232" rx="95" ry="8" fill={primaryColor} opacity={0.08} />

      {/* Shelf line */}
      <Rect x="60" y="228" width="160" height="4" rx="2" fill={primaryColor} opacity={0.2} />

      {/* Stacked books */}
      {books.map((book, i) => (
        <G key={`book-${i}`}>
          {/* Book body */}
          <Rect
            x={book.x}
            y={book.y}
            width={book.w}
            height={book.h}
            rx="4"
            fill={book.color}
            opacity={0.7}
          />
          {/* Spine line */}
          <Rect
            x={book.x + 3}
            y={book.y}
            width="3"
            height={book.h}
            rx="1"
            fill="white"
            opacity={0.3}
          />
          {/* Title on spine */}
          <SvgText
            x={book.x + book.w / 2}
            y={book.y + book.h / 2 + 4}
            fontSize="10"
            fontWeight="600"
            fill="white"
            textAnchor="middle"
            opacity={0.9}
            rotation="-90"
            origin={`${book.x + book.w / 2}, ${book.y + book.h / 2 + 4}`}
          >
            {book.label}
          </SvgText>
        </G>
      ))}

      {/* Floating topic bubbles */}
      {[
        { x: 58, y: 70, label: 'Algebra', color: primaryColor },
        { x: 155, y: 55, label: 'Biology', color: secondaryColor },
        { x: 210, y: 85, label: 'Grammar', color: accentColor },
        { x: 100, y: 45, label: 'Physics', color: secondaryColor },
      ].map((bubble, i) => (
        <G key={`bubble-${i}`} opacity={0.65}>
          <Rect
            x={bubble.x - 24}
            y={bubble.y - 10}
            width="48"
            height="20"
            rx="10"
            fill={bubble.color}
            opacity={0.18}
          />
          <SvgText
            x={bubble.x}
            y={bubble.y + 4}
            fontSize="8"
            fontWeight="600"
            fill={bubble.color}
            textAnchor="middle"
          >
            {bubble.label}
          </SvgText>
        </G>
      ))}

      {/* Upload arrow accent */}
      <G opacity={0.3}>
        <Path
          d="M230 180 L230 160 M224 166 L230 160 L236 166"
          stroke={primaryColor}
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
          fill="none"
        />
        <Rect x="220" y="182" width="20" height="14" rx="3" stroke={primaryColor} strokeWidth="1.5" fill="none" />
      </G>

      {/* Decorative dots */}
      <Circle cx="45" cy="140" r="3" fill={accentColor} opacity={0.2} />
      <Circle cx="235" cy="140" r="3" fill={secondaryColor} opacity={0.2} />
      <Circle cx="140" cy="265" r="3" fill={primaryColor} opacity={0.2} />
    </Svg>
  );
}
