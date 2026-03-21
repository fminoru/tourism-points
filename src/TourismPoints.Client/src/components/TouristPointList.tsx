import BrowserWindow from './BrowserWindow';
import BrandLogo from './BrandLogo';
import type { PaginatedResponse, TouristPoint } from '../services/api';

interface Props {
  activeSearch: string;
  data: PaginatedResponse | null;
  error: string;
  loading: boolean;
  onOpenCreate: () => void;
  onOpenDetails: (point: TouristPoint) => void;
  onOpenHome: () => void;
  onOpenList: () => void;
  onPageChange: (page: number) => void;
  onSearchChange: (value: string) => void;
  onSearchSubmit: () => void;
  search: string;
}

function TouristPointList({
  activeSearch,
  data,
  error,
  loading,
  onOpenCreate,
  onOpenDetails,
  onOpenHome,
  onOpenList,
  onPageChange,
  onSearchChange,
  onSearchSubmit,
  search,
}: Props) {
  const hasResults = Boolean(data?.items.length);
  const hasSearch = Boolean(activeSearch);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    void onSearchSubmit();
  };

  return (
    <BrowserWindow>
      <div className="window-toolbar">
        <BrandLogo alt="Logotipo da aplicação de pontos turísticos" onClick={onOpenHome} />

        <div className="toolbar-actions">
          <button className="button" onClick={onOpenList} type="button">
            listar pontos turísticos
          </button>
          <button className="button button--primary" onClick={onOpenCreate} type="button">
            cadastrar um ponto turístico
          </button>
        </div>
      </div>

      <form className="search-form" onSubmit={handleSubmit}>
        <input
          aria-label="Buscar ponto turístico"
          className="control"
          onChange={(event) => onSearchChange(event.target.value)}
          placeholder="Digite um termo para buscar um ponto turístico..."
          type="search"
          value={search}
        />
        <button className="button" type="submit">
          buscar
        </button>
      </form>

      {error && <div className="feedback feedback--error">{error}</div>}

      {loading ? (
        <div className="feedback feedback--loading">Carregando os registros...</div>
      ) : (
        <>
          <div className="results-summary">
            <div>
              <p className="results-summary__label">Resultados</p>
              <h2>{hasSearch ? `Busca por "${activeSearch}"` : 'Todos os pontos turísticos'}</h2>
            </div>

            <div className="results-summary__meta">
              <span>Total de registros: {data?.totalCount ?? 0}</span>
              <span>
                Pagina {data?.page ?? 1} de {data?.totalPages ?? 1}
              </span>
            </div>
          </div>

          {hasResults ? (
            <>
              <div className="point-list">
                {data?.items.map((point) => (
                  <article className="point-row" key={point.id}>
                    <div className="point-row__main">
                      <div className="point-row__heading">
                        <h3>{point.name}</h3>
                        <p className="point-row__meta">
                          {point.city} - {point.state}
                        </p>
                      </div>
                      <p className="point-row__description">{point.description}</p>
                    </div>

                    <div className="point-row__reference">
                      <span className="point-row__label">Referência</span>
                      <span className="point-row__value">{point.location}</span>
                    </div>

                    <div className="point-row__date">
                      <span className="point-row__label">Criado em</span>
                      <span>{new Date(point.createdAt).toLocaleDateString('pt-BR')}</span>
                    </div>

                    <div className="point-row__actions">
                      <button
                        className="button button--small"
                        onClick={() => onOpenDetails(point)}
                        type="button"
                      >
                        ver detalhes
                      </button>
                    </div>
                  </article>
                ))}
              </div>

              {data && data.totalPages > 1 && (
                <div className="pagination-bar">
                  <button
                    className="text-link"
                    disabled={data.page <= 1}
                    onClick={() => onPageChange(data.page - 1)}
                    type="button"
                  >
                    Voltar
                  </button>

                  <span className="pagination-bar__status">
                    Pagina {data.page} de {data.totalPages}
                  </span>

                  <button
                    className="text-link"
                    disabled={data.page >= data.totalPages}
                    onClick={() => onPageChange(data.page + 1)}
                    type="button"
                  >
                    Avancar
                  </button>
                </div>
              )}
            </>
          ) : (
            <div className="empty-state">
              <p className="empty-state__title">
                Nao encontrei nenhum resultado para a sua busca :(
              </p>
              <p className="empty-state__copy">
                Tente outro termo envolvendo nome, descritivo ou referência do ponto.
              </p>
            </div>
          )}
        </>
      )}
    </BrowserWindow>
  );
}

export default TouristPointList;
