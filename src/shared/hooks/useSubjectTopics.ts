import { useState, useEffect, useCallback } from 'react';
import { enrollmentsAPI, subjectsAPI } from '../../features/courses/api';
import { Enrollment, Topic } from '../../features/courses/types';

export function useSubjectTopics() {
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [topics, setTopics] = useState<Topic[]>([]);
  const [selectedTopic, setSelectedTopic] = useState('');

  const loadEnrollments = useCallback(async () => {
    try {
      const { data } = await enrollmentsAPI.getAll();
      setEnrollments(data);
    } catch (e) {
      console.error('Failed to load enrollments:', e);
    }
  }, []);

  useEffect(() => { loadEnrollments(); }, [loadEnrollments]);

  useEffect(() => {
    if (!selectedSubject) { setTopics([]); setSelectedTopic(''); return; }
    (async () => {
      try {
        const { data } = await subjectsAPI.getTopics(selectedSubject);
        setTopics(data);
        setSelectedTopic('');
      } catch (e) {
        console.error('Failed to load topics:', e);
        setTopics([]);
      }
    })();
  }, [selectedSubject]);

  return {
    enrollments,
    selectedSubject,
    setSelectedSubject,
    topics,
    selectedTopic,
    setSelectedTopic,
    refreshEnrollments: loadEnrollments,
  };
}
