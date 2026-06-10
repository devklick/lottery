import { Center, Grid, Skeleton, Text } from "@mantine/core";
import { Fragment } from "react/jsx-runtime";
import Trophy from "../../components/Trophy/Trophy";

interface GamePrizeObject {
  id: string;
  position: number;
  numberMatchCount: number;
}
interface GamePrizesProps {
  prizes: Array<GamePrizeObject>;
  loading: boolean;
  selectionDrawCount: number;
}

export default function GamePrizes({
  prizes,
  loading,
  selectionDrawCount,
}: GamePrizesProps) {
  return (
    <Grid w={"100%"}>
      {prizes
        .sort((a, b) => a.position - b.position)
        .map((prize, index) => (
          <Fragment key={`prize-${index}`}>
            <Grid.Col key={`prize-${index}-position`} span={6}>
              <Center>
                <Trophy loading={loading} position={prize.position} />
              </Center>
            </Grid.Col>
            <Grid.Col key={`prize-${index}-numberMatchCount`} span={6}>
              <Skeleton visible={loading}>
                <Text>{`${prize.numberMatchCount}/${selectionDrawCount}`}</Text>
              </Skeleton>
            </Grid.Col>
          </Fragment>
        ))}
    </Grid>
  );
}
