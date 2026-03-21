import { useState } from 'react';
import './App.css';
import TouristPointForm from './components/TouristPointForm';
import TouristPointHome from './components/TouristPointHome';
import TouristPointList from './components/TouristPointList';
import { api } from './services/api';
import type {
  PaginatedResponse,
  TouristPoint,
  TouristPointPayload,
} from './services/api';

type View = 'home' | 'results' | 'create' | 'edit' | 'details';

function App() {
  const [view, setView] = useState<View>('home');
  const [selectedPoint, setSelectedPoint] = useState<TouristPoint | null>(null);
  const [data, setData] = useState<PaginatedResponse | null>(null);
  const [searchDraft, setSearchDraft] = useState('');
  const [activeSearch, setActiveSearch] = useState('');
  const [loading, setLoading] = useState(false);
  const [fetchError, setFetchError] = useState('');
  const [submitError, setSubmitError] = useState('');
  const [deleteError, setDeleteError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const fetchPoints = async (page: number = 1, searchTerm: string = activeSearch) => {
    setLoading(true);
    setFetchError('');

    try {
      const result = await api.getAll(page, searchTerm);
      setData(result);
    } catch (error) {
      console.error('Error fetching data:', error);
      setFetchError(
        'Nao foi possivel carregar os pontos turisticos. Verifique a API e tente novamente.',
      );
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setSubmitError('');
    setSelectedPoint(null);
    setView('create');
  };

  const handleGoHome = () => {
    setSubmitError('');
    setDeleteError('');
    setSelectedPoint(null);
    setSearchDraft('');
    setActiveSearch('');
    setView('home');
  };

  const handleOpenList = async () => {
    setSubmitError('');
    setDeleteError('');
    setSelectedPoint(null);
    setSearchDraft('');
    setActiveSearch('');
    setView('results');
    await fetchPoints(1, '');
  };

  const handleEdit = (point: TouristPoint) => {
    setSubmitError('');
    setDeleteError('');
    setSelectedPoint(point);
    setView('edit');
  };

  const handleViewDetails = (point: TouristPoint) => {
    setDeleteError('');
    setSelectedPoint(point);
    setView('details');
  };

  const handleReturn = () => {
    setSubmitError('');
    setDeleteError('');
    setSelectedPoint(null);
    setView(data ? 'results' : 'home');
  };

  const handleSearchSubmit = async () => {
    const nextSearch = searchDraft.trim();
    setActiveSearch(nextSearch);
    setView('results');
    await fetchPoints(1, nextSearch);
  };

  const handlePageChange = async (page: number) => {
    await fetchPoints(page, activeSearch);
  };

  const handleSave = async (payload: TouristPointPayload) => {
    setIsSubmitting(true);
    setSubmitError('');

    try {
      if (view === 'edit' && selectedPoint) {
        await api.update(selectedPoint.id, payload);
      } else {
        await api.create(payload);
      }

      const currentPage = data?.page ?? 1;
      await fetchPoints(currentPage, activeSearch);
      setSelectedPoint(null);
      setView('results');
    } catch (error) {
      console.error('Error saving tourist point:', error);
      setSubmitError(
        'Nao foi possivel salvar o ponto turistico. Confira os dados e tente novamente.',
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!selectedPoint) {
      return;
    }

    const shouldDelete = window.confirm(
      `Deseja excluir "${selectedPoint.name}" da lista de pontos turisticos?`,
    );

    if (!shouldDelete) {
      return;
    }

    setIsDeleting(true);
    setDeleteError('');

    try {
      await api.delete(selectedPoint.id);

      const currentPage = data?.page ?? 1;
      const pageSize = data?.pageSize ?? 10;
      const remainingCount = Math.max((data?.totalCount ?? 1) - 1, 0);
      const lastAvailablePage = Math.max(Math.ceil(remainingCount / pageSize), 1);
      const nextPage = Math.min(currentPage, lastAvailablePage);

      await fetchPoints(nextPage, activeSearch);
      setSelectedPoint(null);
      setView('results');
    } catch (error) {
      console.error('Error deleting tourist point:', error);
      setDeleteError(
        'Nao foi possivel excluir este cadastro agora. Tente novamente em instantes.',
      );
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <div className="app-shell">
      <main className="app-main">
        {view === 'home' && (
          <TouristPointHome
            onOpenCreate={handleCreate}
            onOpenHome={handleGoHome}
            onOpenList={handleOpenList}
            onSearchChange={setSearchDraft}
            onSearchSubmit={handleSearchSubmit}
            search={searchDraft}
          />
        )}

        {view === 'results' && (
          <TouristPointList
            activeSearch={activeSearch}
            data={data}
            error={fetchError}
            loading={loading}
            onOpenCreate={handleCreate}
            onOpenDetails={handleViewDetails}
            onOpenHome={handleGoHome}
            onOpenList={handleOpenList}
            onPageChange={handlePageChange}
            onSearchChange={setSearchDraft}
            onSearchSubmit={handleSearchSubmit}
            search={searchDraft}
          />
        )}

        {(view === 'create' || view === 'edit') && (
          <TouristPointForm
            error={submitError}
            key={selectedPoint?.id ?? view}
            mode={view === 'edit' ? 'edit' : 'create'}
            onCancel={handleReturn}
            onGoHome={handleGoHome}
            onSave={handleSave}
            point={selectedPoint}
            saving={isSubmitting}
          />
        )}

        {view === 'details' && selectedPoint && (
          <TouristPointForm
            deleting={isDeleting}
            error={deleteError}
            key={`details-${selectedPoint.id}`}
            mode="details"
            onCancel={handleReturn}
            onDelete={handleDelete}
            onEdit={() => handleEdit(selectedPoint)}
            onGoHome={handleGoHome}
            point={selectedPoint}
          />
        )}
      </main>
    </div>
  );
}

export default App;
