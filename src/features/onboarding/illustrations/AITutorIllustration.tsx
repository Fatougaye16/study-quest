import React from 'react';
import Svg, { G, Circle, Path, Rect, Ellipse } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function AITutorIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Large chat bubble — AI response */}
      <G>
        <Rect x="55" y="75" width="150" height="70" rx="16" fill={secondaryColor} opacity={0.15} />
        <Path d="M80 145 L70 160 L95 145" fill={secondaryColor} opacity={0.15} />
        {/* Lines of "text" inside */}
        <Rect x="72" y="92" width="100" height="5" rx="2.5" fill={secondaryColor} opacity={0.3} />
        <Rect x="72" y="103" width="80" height="5" rx="2.5" fill={secondaryColor} opacity={0.25} />
        <Rect x="72" y="114" width="110" height="5" rx="2.5" fill={secondaryColor} opacity={0.2} />
        <Rect x="72" y="125" width="60" height="5" rx="2.5" fill={secondaryColor} opacity={0.15} />
      </G>

      {/* Small chat bubble — student question */}
      <G>
        <Rect x="130" y="170" width="100" height="40" rx="12" fill={primaryColor} opacity={0.2} />
        <Path d="M205 210 L215 222 L195 210" fill={primaryColor} opacity={0.2} />
        {/* Text lines */}
        <Rect x="145" y="183" width="70" height="5" rx="2.5" fill={primaryColor} opacity={0.35} />
        <Rect x="145" y="194" width="45" height="5" rx="2.5" fill={primaryColor} opacity={0.25} />
      </G>

      {/* Brain / AI icon at top-left */}
      <G opacity={0.6}>
        <Circle cx="50" cy="55" r="18" fill={secondaryColor} opacity={0.15} />
        {/* Simplified brain paths */}
        <Path
          d="M43 55 Q43 48 50 48 Q57 48 57 55 Q57 62 50 62 Q43 62 43 55"
          stroke={secondaryColor}
          strokeWidth="1.8"
          fill="none"
        />
        <Path
          d="M50 48 L50 62 M43 52 Q47 55 43 58 M57 52 Q53 55 57 58"
          stroke={secondaryColor}
          strokeWidth="1.5"
          fill="none"
          strokeLinecap="round"
        />
      </G>

      {/* Sparkle accents */}
      {[
        { x: 220, y: 65 },
        { x: 240, y: 105 },
        { x: 45, y: 190 },
      ].map((s, i) => (
        <G key={`sparkle-${i}`} opacity={0.4}>
          <Path
            d={`M${s.x} ${s.y - 6} L${s.x} ${s.y + 6} M${s.x - 6} ${s.y} L${s.x + 6} ${s.y}`}
            stroke={accentColor}
            strokeWidth="2"
            strokeLinecap="round"
          />
          <Path
            d={`M${s.x - 4} ${s.y - 4} L${s.x + 4} ${s.y + 4} M${s.x + 4} ${s.y - 4} L${s.x - 4} ${s.y + 4}`}
            stroke={accentColor}
            strokeWidth="1"
            strokeLinecap="round"
          />
        </G>
      ))}

      {/* Lightning bolt — "instant answers" */}
      <G opacity={0.35}>
        <Path
          d="M235 155 L228 170 L234 170 L227 185"
          stroke={accentColor}
          strokeWidth="2.5"
          strokeLinecap="round"
          strokeLinejoin="round"
          fill="none"
        />
      </G>

      {/* Floating quiz icon */}
      <G opacity={0.3}>
        <Rect x="40" y="230" width="24" height="28" rx="4" stroke={primaryColor} strokeWidth="1.5" fill="none" />
        <Path d="M46 240 L58 240 M46 247 L54 247" stroke={primaryColor} strokeWidth="1.5" strokeLinecap="round" />
        <Circle cx="48" cy="253" r="2" fill={primaryColor} />
      </G>
    </Svg>
  );
}
