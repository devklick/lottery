import { useParams } from "react-router-dom";

interface Params extends Record<string, string | undefined> {
  id: string;
}

interface GameDetailProps {}

function GameDetail({}: GameDetailProps) {
  const { id } = useParams<Params>();
  return `hello from game detail ${id}`;
}

export default GameDetail;
