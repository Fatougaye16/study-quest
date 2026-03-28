import React, { useRef, useState, useCallback, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  Dimensions,
  FlatList,
  Animated,
  TouchableOpacity,
  StatusBar,
  ViewToken,
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { useTheme } from '../../shared/theme';
import { useAuth } from '../auth/context';
import { slides, OnboardingSlide } from './slides';
import WelcomeIllustration from './illustrations/WelcomeIllustration';
import TimetableIllustration from './illustrations/TimetableIllustration';
import CoursesIllustration from './illustrations/CoursesIllustration';
import StudyPlanIllustration from './illustrations/StudyPlanIllustration';
import AITutorIllustration from './illustrations/AITutorIllustration';
import ProgressIllustration from './illustrations/ProgressIllustration';
import LetsGoIllustration from './illustrations/LetsGoIllustration';

const { width: SCREEN_WIDTH, height: SCREEN_HEIGHT } = Dimensions.get('window');

interface Props {
  onComplete: () => void;
}

const illustrationMap: Record<OnboardingSlide['illustrationKey'], React.FC<{
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}>> = {
  welcome: WelcomeIllustration,
  timetable: TimetableIllustration,
  courses: CoursesIllustration,
  studyPlan: StudyPlanIllustration,
  aiTutor: AITutorIllustration,
  progress: ProgressIllustration,
  letsGo: LetsGoIllustration,
};

export default function OnboardingScreen({ onComplete }: Props) {
  const { theme } = useTheme();
  const { user } = useAuth();
  const insets = useSafeAreaInsets();
  const flatListRef = useRef<FlatList>(null);
  const scrollX = useRef(new Animated.Value(0)).current;
  const [currentIndex, setCurrentIndex] = useState(0);

  // Per-slide animated values for entrance animations
  const titleOpacity = useRef(new Animated.Value(0)).current;
  const titleTranslateY = useRef(new Animated.Value(20)).current;
  const subtitleOpacity = useRef(new Animated.Value(0)).current;
  const subtitleTranslateY = useRef(new Animated.Value(15)).current;
  const illustrationScale = useRef(new Animated.Value(0.85)).current;
  const illustrationOpacity = useRef(new Animated.Value(0)).current;

  const animateSlideIn = useCallback(() => {
    // Reset
    titleOpacity.setValue(0);
    titleTranslateY.setValue(20);
    subtitleOpacity.setValue(0);
    subtitleTranslateY.setValue(15);
    illustrationScale.setValue(0.85);
    illustrationOpacity.setValue(0);

    // Staggered entrance
    Animated.stagger(120, [
      Animated.parallel([
        Animated.timing(illustrationOpacity, { toValue: 1, duration: 400, useNativeDriver: true }),
        Animated.spring(illustrationScale, { toValue: 1, friction: 8, tension: 60, useNativeDriver: true }),
      ]),
      Animated.parallel([
        Animated.timing(titleOpacity, { toValue: 1, duration: 350, useNativeDriver: true }),
        Animated.spring(titleTranslateY, { toValue: 0, friction: 8, tension: 60, useNativeDriver: true }),
      ]),
      Animated.parallel([
        Animated.timing(subtitleOpacity, { toValue: 1, duration: 350, useNativeDriver: true }),
        Animated.spring(subtitleTranslateY, { toValue: 0, friction: 8, tension: 60, useNativeDriver: true }),
      ]),
    ]).start();
  }, [titleOpacity, titleTranslateY, subtitleOpacity, subtitleTranslateY, illustrationScale, illustrationOpacity]);

  useEffect(() => {
    animateSlideIn();
  }, [currentIndex, animateSlideIn]);

  const onViewableItemsChanged = useRef(({ viewableItems }: { viewableItems: ViewToken[] }) => {
    if (viewableItems.length > 0 && viewableItems[0].index != null) {
      setCurrentIndex(viewableItems[0].index);
    }
  }).current;

  const viewabilityConfig = useRef({ viewAreaCoveragePercentThreshold: 50 }).current;

  const goToNext = () => {
    if (currentIndex < slides.length - 1) {
      flatListRef.current?.scrollToIndex({ index: currentIndex + 1, animated: true });
    } else {
      onComplete();
    }
  };

  const skip = () => {
    onComplete();
  };

  const isLastSlide = currentIndex === slides.length - 1;

  // Personalize the welcome title
  const getTitle = (slide: OnboardingSlide) => {
    if (slide.id === 'welcome' && user?.firstName) {
      return `Welcome, ${user.firstName}!`;
    }
    return slide.title;
  };

  const renderSlide = ({ item, index }: { item: OnboardingSlide; index: number }) => {
    const IllustrationComponent = illustrationMap[item.illustrationKey];

    return (
      <View style={[styles.slide, { width: SCREEN_WIDTH }]}>
        {/* Illustration area */}
        <View style={styles.illustrationContainer}>
          <Animated.View
            style={[
              styles.illustrationWrapper,
              index === currentIndex
                ? { opacity: illustrationOpacity, transform: [{ scale: illustrationScale }] }
                : { opacity: 0.3 },
            ]}
          >
            <IllustrationComponent
              size={SCREEN_WIDTH * 0.72}
              primaryColor={theme.colors.primary}
              secondaryColor={theme.colors.secondary}
              accentColor={theme.colors.accent}
            />
          </Animated.View>
        </View>

        {/* Text area */}
        <View style={styles.textContainer}>
          <Animated.Text
            style={[
              styles.title,
              {
                color: theme.colors.text,
                fontFamily: theme.fonts.headingBold,
              },
              index === currentIndex
                ? { opacity: titleOpacity, transform: [{ translateY: titleTranslateY }] }
                : {},
            ]}
          >
            {getTitle(item)}
          </Animated.Text>
          <Animated.Text
            style={[
              styles.subtitle,
              {
                color: theme.colors.textSecondary,
                fontFamily: theme.fonts.body,
              },
              index === currentIndex
                ? { opacity: subtitleOpacity, transform: [{ translateY: subtitleTranslateY }] }
                : {},
            ]}
          >
            {item.subtitle}
          </Animated.Text>
        </View>
      </View>
    );
  };

  // Pulsing animation for the "Let's Go" button
  const pulseAnim = useRef(new Animated.Value(1)).current;
  useEffect(() => {
    if (isLastSlide) {
      const pulse = Animated.loop(
        Animated.sequence([
          Animated.timing(pulseAnim, { toValue: 1.05, duration: 800, useNativeDriver: true }),
          Animated.timing(pulseAnim, { toValue: 1, duration: 800, useNativeDriver: true }),
        ])
      );
      pulse.start();
      return () => pulse.stop();
    } else {
      pulseAnim.setValue(1);
    }
  }, [isLastSlide, pulseAnim]);

  return (
    <View style={[styles.container, { backgroundColor: theme.colors.background }]}>  
      <StatusBar barStyle={theme.colors.background === '#F7FAFC' ? 'dark-content' : 'light-content'} />

      {/* Skip button */}
      {!isLastSlide && (
        <TouchableOpacity
          style={[styles.skipButton, { top: insets.top + 12 }]}
          onPress={skip}
          activeOpacity={0.7}
        >
          <Text style={[styles.skipText, { color: theme.colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>
            Skip
          </Text>
        </TouchableOpacity>
      )}

      {/* Slides */}
      <FlatList
        ref={flatListRef}
        data={slides}
        renderItem={renderSlide}
        keyExtractor={(item) => item.id}
        horizontal
        pagingEnabled
        showsHorizontalScrollIndicator={false}
        bounces={false}
        onScroll={Animated.event(
          [{ nativeEvent: { contentOffset: { x: scrollX } } }],
          { useNativeDriver: false }
        )}
        onViewableItemsChanged={onViewableItemsChanged}
        viewabilityConfig={viewabilityConfig}
        scrollEventThrottle={16}
      />

      {/* Bottom controls */}
      <View style={[styles.bottomBar, { paddingBottom: insets.bottom + 20 }]}>
        {/* Progress dots */}
        <View style={styles.dotsContainer}>
          {slides.map((_, i) => {
            const inputRange = [(i - 1) * SCREEN_WIDTH, i * SCREEN_WIDTH, (i + 1) * SCREEN_WIDTH];

            const dotWidth = scrollX.interpolate({
              inputRange,
              outputRange: [8, 24, 8],
              extrapolate: 'clamp',
            });

            const dotOpacity = scrollX.interpolate({
              inputRange,
              outputRange: [0.3, 1, 0.3],
              extrapolate: 'clamp',
            });

            return (
              <Animated.View
                key={`dot-${i}`}
                style={[
                  styles.dot,
                  {
                    width: dotWidth,
                    opacity: dotOpacity,
                    backgroundColor: theme.colors.primary,
                  },
                ]}
              />
            );
          })}
        </View>

        {/* Action button */}
        <Animated.View style={isLastSlide ? { transform: [{ scale: pulseAnim }] } : undefined}>
          <TouchableOpacity
            style={[
              styles.actionButton,
              {
                backgroundColor: theme.colors.primary,
                minWidth: isLastSlide ? 200 : 140,
              },
            ]}
            onPress={goToNext}
            activeOpacity={0.8}
          >
            <Text style={[styles.actionButtonText, { fontFamily: theme.fonts.bodySemiBold }]}>
              {isLastSlide ? "Start Learning" : 'Next'}
            </Text>
          </TouchableOpacity>
        </Animated.View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  skipButton: {
    position: 'absolute',
    right: 20,
    zIndex: 10,
    paddingHorizontal: 12,
    paddingVertical: 8,
  },
  skipText: {
    fontSize: 15,
  },
  slide: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  illustrationContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'flex-end',
    paddingBottom: 16,
  },
  illustrationWrapper: {
    alignItems: 'center',
    justifyContent: 'center',
  },
  textContainer: {
    flex: 0.55,
    alignItems: 'center',
    paddingHorizontal: 36,
    justifyContent: 'flex-start',
  },
  title: {
    fontSize: 28,
    fontWeight: '700',
    textAlign: 'center',
    marginBottom: 12,
  },
  subtitle: {
    fontSize: 16,
    textAlign: 'center',
    lineHeight: 24,
    paddingHorizontal: 8,
  },
  bottomBar: {
    alignItems: 'center',
    paddingTop: 8,
  },
  dotsContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 24,
  },
  dot: {
    height: 8,
    borderRadius: 4,
    marginHorizontal: 4,
  },
  actionButton: {
    paddingHorizontal: 32,
    paddingVertical: 16,
    borderRadius: 16,
    alignItems: 'center',
  },
  actionButtonText: {
    color: '#FFFFFF',
    fontSize: 17,
    fontWeight: '600',
  },
});
