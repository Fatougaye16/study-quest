import { useState, useCallback } from 'react';
import * as FileSystem from 'expo-file-system';
import * as Sharing from 'expo-sharing';
import { Alert, Platform } from 'react-native';
import { getAccessToken } from '../api/client';

const BASE_URL = 'https://study-quest-production.up.railway.app';

type DownloadType = 'past-paper' | 'notes' | 'flashcards' | 'study-plan';

export function useDownload() {
  const [downloading, setDownloading] = useState(false);

  const download = useCallback(async (type: DownloadType, id: string, fallbackName?: string) => {
    setDownloading(true);
    try {
      const token = await getAccessToken();
      if (!token) {
        Alert.alert('Error', 'Please log in again.');
        return;
      }

      const url = `${BASE_URL}/api/downloads/${type}/${id}`;
      const fileName = fallbackName || `StudyQuest_${type}_${id.slice(0, 8)}.pdf`;
      const fileUri = `${FileSystem.cacheDirectory}${fileName}`;

      const result = await FileSystem.downloadAsync(url, fileUri, {
        headers: { Authorization: `Bearer ${token}` },
      });

      if (result.status !== 200) {
        Alert.alert('Download Failed', 'Could not download the file. Please try again.');
        return;
      }

      const sharingAvailable = await Sharing.isAvailableAsync();
      if (sharingAvailable) {
        await Sharing.shareAsync(result.uri, {
          mimeType: 'application/pdf',
          dialogTitle: `Save ${fileName}`,
        });
      } else {
        Alert.alert('Downloaded', `File saved to: ${result.uri}`);
      }
    } catch (error: any) {
      console.error('Download error:', error);
      Alert.alert('Download Error', error.message || 'Something went wrong.');
    } finally {
      setDownloading(false);
    }
  }, []);

  return { download, downloading };
}
