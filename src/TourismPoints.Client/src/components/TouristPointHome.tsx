import BrandLogo from './BrandLogo';
import BrowserWindow from './BrowserWindow';

interface Props {
  onOpenCreate: () => void;
  onOpenList: () => void;
  onOpenHome: () => void;
  onSearchChange: (value: string) => void;
  onSearchSubmit: () => void;
  search: string;
}

function TouristPointHome({
  onOpenCreate,
  onOpenHome,
  onOpenList,
  onSearchChange,
  onSearchSubmit,
  search,
}: Props) {
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    void onSearchSubmit();
  };

  return (
    <BrowserWindow className="browser-window--home">
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
    </BrowserWindow>
  );
}

export default TouristPointHome;
