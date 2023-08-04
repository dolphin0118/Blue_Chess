using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAstar : MonoBehaviour {

    List<Vector3Int> Tile_Pos;
    public List<List<bool>> isBattle;
    //public Block[,] blocks;
    public Board board;
    public int Lerp_x = 4;
    public int Lerp_y = 6;
    public int width = 9;
    public int height = 7;

    Vector2Int Coord_Lerp() {
        Vector3Int pos = this.GetComponent<CharaLocate>().Player_Tilepos();
        int Lerp_pos_x = pos.x + Lerp_x;
        int Lerp_pos_y = pos.y + Lerp_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_pos_x, Lerp_pos_y);
        return Lerp_pos;
    }

    void Start() {
        board = new Board(width, height);
        Blocks_set();
    }

    void Blocks_set() {
        board.blocks = new Block[width, height];
        for (int i = 0; i < board.blocks.GetLength(0); i++) {
            for (int j = 0; j < board.blocks.GetLength(1); j++) {
                board.blocks[i, j] = new Block(i, j);
            }
        }
    }

    Vector2Int Target_Coord_Lerp() {
        Vector3Int pos = this.GetComponent<CharaController>().Set_Target();
        int Lerp_pos_x = pos.x + Lerp_x;
        int Lerp_pos_y = pos.y + Lerp_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_pos_x, Lerp_pos_y);
        return Lerp_pos;
    }

    bool Exists(int x, int y) {
        if(x >= 0 && y >= 0 && isBattle[x][y]) return true;
        else return false;
    }

    void TileCheck() {
        isBattle = MapManager.instance.isBattle;
        Vector2Int target = Coord_Lerp();
        List<Block> arounds = new List<Block>();
        if (Exists(target.x - 1, target.y - 1)) {
            Block block = board.blocks[target.x - 1, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y - 1))
        {
            Block block = board.blocks[target.x, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y - 1))
        {
            Block block = board.blocks[target.x + 1, target.y - 1];
            arounds.Add(block);
        }

        if (Exists(target.x - 1, target.y))
        {
            Block block = board.blocks[target.x - 1, target.y];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y))
        {
            Block block = board.blocks[target.x + 1, target.y];
            arounds.Add(block);
        }

        if (Exists(target.x - 1, target.y + 1))
        {
            Block block = board.blocks[target.x - 1, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y + 1))
        {
            Block block = board.blocks[target.x, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y + 1))
        {
            Block block = board.blocks[target.x + 1, target.y + 1];
            arounds.Add(block);
        }
    }
  
    void Update() {
    }

    void MovePath() {
        Vector2Int start_pos = Coord_Lerp();
        Vector2Int dest_pos = Target_Coord_Lerp();
        if(start_pos.x >= 0 && start_pos.y >= 0) {
            TileCheck();
            Block start = board.blocks[start_pos.x, start_pos.y];
            Block dest = board.blocks[dest_pos.x, dest_pos.y];
            var startBlock = PathFindTile(start,dest);
            while(startBlock != null) {
                Debug.Log(startBlock.x);
                Debug.Log(startBlock.y);
                if(startBlock != null) startBlock = startBlock.next;
            }

        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Main") {
            MovePath();
        }
    }

    Block PathFindTile(Block start, Block dest) {   
        if(board.Exists(start) && board.Exists(dest)) {
            board.CheckClear();
            List<Block> waittingBlocks = new List<Block>();
            List<Block> finishedBlocks = new List<Block>();
            Block currentBlock = start;

            while(currentBlock != null) {
                var aroundBlocks = board.GetAroundBlocks(currentBlock);

                for(int i = 0; i < aroundBlocks.Count; i++) {
                    var block = aroundBlocks[i];
                    if(!waittingBlocks.Equals(block) && !block.check) {
                        waittingBlocks.Add(block);
                    }
                }

                currentBlock.check = true;
                if(waittingBlocks.Remove(currentBlock)) finishedBlocks.Add(currentBlock);

                if(aroundBlocks.Count == 0) return null;
                else {
                    aroundBlocks = aroundBlocks.FindAll(block => !block.check);
                }

                CalcRating(aroundBlocks, start, currentBlock, dest);

                currentBlock = GetNextBlock(aroundBlocks, currentBlock);
                if(currentBlock == null) {
                    currentBlock = GetPriorityBlock(waittingBlocks);

                    if(currentBlock == null) {
                        Block exceptionBlock = null;
                        for(int i = 0; i < finishedBlocks.Count; i++) {
                            if(exceptionBlock == null || exceptionBlock.H > finishedBlocks[i].H) exceptionBlock = finishedBlocks[i];

                        }
                        currentBlock = exceptionBlock;
                        break;
                    }
                }
                else if(currentBlock.Equals(dest)) break;
            }
            while(!currentBlock.Equals(start)) {
                currentBlock.prev.next = currentBlock;
                currentBlock = currentBlock.prev; 
            }

            start.prev = null;
            return start;
        }
        return null;
    }

    Block GetPriorityBlock(List<Block> waittingBlocks) {
        Block block = null;
        var enumerator = waittingBlocks.GetEnumerator();
        while(enumerator.MoveNext()) {
            var current = enumerator.Current;
            if(block == null || block.F < current.F) block = current;
        }
        return block;
    }

    Block GetNextBlock(List<Block> arounds, Block current) {
        Block minValueBlock = null;
        
        for(int i = 0; i < arounds.Count; i++) {
            Block next = arounds[i];
            if(!next.check) {
                if(minValueBlock == null) minValueBlock = next;
                else if(minValueBlock.H > next.H) minValueBlock = next;
                else continue;
            }
        }
    
        return minValueBlock;
    }

    void CalcRating(List<Block> arounds, Block start, Block current, Block dest) {
        if(arounds !=null) {
            for(int i = 0; i < arounds.Count; i++) {
                var block = arounds[i];
                bool isDiagonalBlock = Math.Abs(block.x - current.x) == 1 && Math.Abs(block.y - current.y) == 1;
                int priceFromDest = (Math.Abs(dest.x - block.x) + Math.Abs(dest.y - block.y)) * 10;
                if (block.prev == null) block.prev = current;
                block.SetPrice(current.G + (isDiagonalBlock ? 14 : 10), priceFromDest);
            }
        }
    }
}
